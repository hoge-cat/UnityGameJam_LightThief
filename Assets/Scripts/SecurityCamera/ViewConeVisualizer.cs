using UnityEngine;
using UnityEngine.Rendering;

public class ViewConeVisualizer : MonoBehaviour
{
    public enum ViewState
    {
        Normal,
        Warning,
        Alert
    }

    [Header("参照")]
    [SerializeField] private Transform visionOrigin;

    [Header("視界設定")]
    [SerializeField] private float viewDistance = 8.0f;

    [SerializeField, Range(0.0f, 360.0f)]
    private float viewAngle = 60.0f;

    [Header("表示設定")]
    [SerializeField] private Material viewMaterial;

    [SerializeField] private float heightOffset = 0.03f;
    [SerializeField] private float groundY = 0.03f;

    [SerializeField, Range(3, 100)]
    private int segmentCount = 30;

    [Header("色設定")]
    [SerializeField]
    private Color normalColor =
        new Color(1.0f, 1.0f, 0.0f, 0.3f);

    [SerializeField]
    private Color warningColor =
        new Color(1.0f, 0.5f, 0.0f, 0.3f);

    [SerializeField]
    private Color alertColor =
        new Color(1.0f, 0.0f, 0.0f, 0.3f);

    private Mesh viewMesh;
    private GameObject viewObject;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        CreateViewCone();
        SetViewState(ViewState.Normal);
    }

    private void LateUpdate()
    {
        if (viewObject == null)
        {
            return;
        }

        Vector3 viewPosition;

        if (visionOrigin != null)
        {
            // X・Zは実際の視界判定の開始位置に合わせる
            viewPosition = visionOrigin.position;
        }
        else
        {
            viewPosition = transform.position;
        }

        // Yだけ地面の高さに固定する
        viewPosition.y = groundY + heightOffset;

        viewObject.transform.position = viewPosition;

        float yRotation = transform.eulerAngles.y;

        if (visionOrigin != null)
        {
            // 向きも実際の判定と合わせる
            yRotation = visionOrigin.eulerAngles.y;
        }

        viewObject.transform.rotation =
            Quaternion.Euler(0.0f, yRotation, 0.0f);
    }

    private void CreateViewCone()
    {
        viewObject =
            new GameObject("ViewConeVisual");

        meshRenderer =
            viewObject.AddComponent<MeshRenderer>();

        MeshFilter meshFilter =
            viewObject.AddComponent<MeshFilter>();

        viewMesh = new Mesh();
        viewMesh.name = "ViewConeMesh";

        meshFilter.mesh = viewMesh;

        if (viewMaterial != null)
        {
            meshRenderer.material = viewMaterial;
        }

        meshRenderer.shadowCastingMode =
            ShadowCastingMode.Off;

        meshRenderer.receiveShadows = false;

        CreateMesh();
    }

    private void CreateMesh()
    {
        if (viewMesh == null)
        {
            return;
        }

        Vector3[] vertices =
            new Vector3[segmentCount + 2];

        int[] triangles =
            new int[segmentCount * 3];

        vertices[0] = Vector3.zero;

        float startAngle =
            -viewAngle / 2.0f;

        float angleStep =
            viewAngle / segmentCount;

        for (int i = 0; i <= segmentCount; i++)
        {
            float currentAngle =
                startAngle + angleStep * i;

            float radian =
                currentAngle * Mathf.Deg2Rad;

            float x =
                Mathf.Sin(radian) * viewDistance;

            float z =
                Mathf.Cos(radian) * viewDistance;

            vertices[i + 1] =
                new Vector3(x, 0.0f, z);
        }

        for (int i = 0; i < segmentCount; i++)
        {
            int triangleIndex =
                i * 3;

            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] = i + 1;
            triangles[triangleIndex + 2] = i + 2;
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;

        viewMesh.RecalculateNormals();
        viewMesh.RecalculateBounds();
    }

    public void SetViewState(ViewState state)
    {
        if (meshRenderer == null)
        {
            return;
        }

        Color targetColor;

        switch (state)
        {
            case ViewState.Warning:
                targetColor = warningColor;
                break;

            case ViewState.Alert:
                targetColor = alertColor;
                break;

            default:
                targetColor = normalColor;
                break;
        }

        meshRenderer.material.color = targetColor;
    }

    private void OnDestroy()
    {
        if (viewMesh != null)
        {
            Destroy(viewMesh);
        }

        if (viewObject != null)
        {
            Destroy(viewObject);
        }
    }
}