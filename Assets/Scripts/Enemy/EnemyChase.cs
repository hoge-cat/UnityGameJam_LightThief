using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private Transform player;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void UpdateChase()
    {
        if (player == null)
        {
            return;
        }

        agent.SetDestination(player.position);
    }

    public void StopChase()
    {
        if (agent.hasPath)
        {
            agent.ResetPath();
        }
    }
}