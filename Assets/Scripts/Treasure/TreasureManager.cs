using TMPro;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager Instance { get; private set; }

    [SerializeField] private TMP_Text treasureText;

    [Header("ゴール設定")]
    [SerializeField] private GameObject goalObject;

    private int collectedTreasure;
    private int totalTreasure;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TreasureItem[] treasures =
            FindObjectsByType<TreasureItem>(
                FindObjectsSortMode.None
            );

        totalTreasure = treasures.Length;

        if (goalObject != null)
        {
            goalObject.SetActive(false);
        }

        UpdateTreasureUI();
    }

    public void CollectTreasure()
    {
        collectedTreasure++;

        UpdateTreasureUI();

        if (HasCollectedAllTreasure())
        {
            ActivateGoal();
        }
    }

    public int GetCollectedTreasure()
    {
        return collectedTreasure;
    }

    public int GetTotalTreasure()
    {
        return totalTreasure;
    }

    public bool HasCollectedAllTreasure()
    {
        return totalTreasure > 0 &&
               collectedTreasure >= totalTreasure;
    }

    private void UpdateTreasureUI()
    {
        if (treasureText == null)
        {
            return;
        }

        treasureText.text =
            "TREASURE\n" +
            collectedTreasure +
            " / " +
            totalTreasure;
    }

    private void ActivateGoal()
    {
        if (goalObject != null)
        {
            goalObject.SetActive(true);
        }

        Debug.Log("すべての宝を集めました。出口が有効になりました。");
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}