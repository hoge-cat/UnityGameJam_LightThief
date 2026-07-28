using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private Transform player;

    public bool IsChasing { get; private set; }

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("Playerタグのオブジェクトが見つかりません。");
            }
        }
    }

    public void UpdateChase()
    {
        if (player == null)
        {
            return;
        }

        if (agent == null || !agent.isOnNavMesh)
        {
            return;
        }

        IsChasing = true;

        agent.SetDestination(player.position);
    }

    public void StopChase()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            return;
        }

        IsChasing = false;

        if (agent.hasPath)
        {
            agent.ResetPath();
        }
    }
}