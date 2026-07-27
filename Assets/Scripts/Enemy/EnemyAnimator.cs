using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int AttackHash =
        Animator.StringToHash("Attack");

    public void SetSpeed(float speed)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetFloat(SpeedHash, speed);
    }

    public void PlayAttack()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger(AttackHash);
    }
}