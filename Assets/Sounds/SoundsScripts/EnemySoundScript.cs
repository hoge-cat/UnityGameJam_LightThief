using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public AudioClip wingSE;
    public AudioClip attackSE;

    [SerializeField] private Transform player;
    public float attackRange = 2.0f;

    private AudioSource audioSource;
    private EnemyChase chase;

    public float attackInterval = 1.0f;
    private float timer;

    public float hearDistance = 30f;     // 聞こえ始める距離
    public float maxVolumeDistance = 5f; // 最大音量になる距離

    public float patrolVolume = 0.5f;
    public float chaseVolume = 1.0f;

    public float patrolPitch = 1.0f;
    public float chasePitch = 1.15f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        chase = GetComponent<EnemyChase>();

        // Playerを自動取得
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // 羽音をループ再生
        audioSource.clip = wingSE;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.pitch = patrolPitch;
        audioSource.Play();
    }

    void Update()
    {
        if (player == null)
            return;

        // プレイヤーとの距離
        float distance = Vector3.Distance(transform.position, player.position);

        // 距離によるフェード
        float distanceVolume = Mathf.InverseLerp(
            hearDistance,
            maxVolumeDistance,
            distance
        );

        // 巡回・追跡による音量変化
        float stateVolume;

        if (chase != null && chase.IsChasing)
        {
            stateVolume = chaseVolume;
            audioSource.pitch = chasePitch;
        }
        else
        {
            stateVolume = patrolVolume;
            audioSource.pitch = patrolPitch;
        }

        // 最終的な音量
        audioSource.volume = distanceVolume * stateVolume;
    }

    public void PlayAttackSE()
    {
        Debug.Log("攻撃音呼び出し");
        StartCoroutine(PlayAttackSEDelay());
    }

    private IEnumerator PlayAttackSEDelay()
    {
        yield return new WaitForSeconds(0.3f);

        audioSource.PlayOneShot(attackSE);
    }
}