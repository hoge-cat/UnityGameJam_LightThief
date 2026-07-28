using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource dashSource;
    [SerializeField] private AudioSource seSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip footstepSE;
    [SerializeField] private AudioClip jumpSE;
    [SerializeField] private AudioClip landingSE;
    [SerializeField] private AudioClip dashSE;

    [Header("足音間隔")]
    [SerializeField] private float walkInterval = 0.45f;
    [SerializeField] private float dashInterval = 0.22f;

    private float walkTimer;
    private float dashTimer;

    public void PlayFootstep()
    {
        if (footstepSource == null || footstepSE == null)
        {
            return;
        }

        walkTimer -= Time.fixedDeltaTime;

        if (walkTimer <= 0.0f)
        {
            footstepSource.Stop();
            footstepSource.clip = footstepSE;
            footstepSource.loop = false;
            footstepSource.Play();

            walkTimer = walkInterval;
        }
    }

    public void PlayDash()
    {
        if (dashSource == null || dashSE == null)
        {
            return;
        }

        dashTimer -= Time.fixedDeltaTime;

        if (dashTimer <= 0.0f)
        {
            dashSource.Stop();
            dashSource.clip = dashSE;
            dashSource.loop = false;
            dashSource.Play();

            dashTimer = dashInterval;
        }
    }

    public void StopFootstep()
    {
        walkTimer = 0.0f;

        if (footstepSource != null)
        {
            footstepSource.Stop();
        }
    }

    public void StopDash()
    {
        dashTimer = 0.0f;

        if (dashSource != null)
        {
            dashSource.Stop();
        }
    }

    public void PlayJump()
    {
        if (seSource != null && jumpSE != null)
        {
            seSource.PlayOneShot(jumpSE);
        }
    }

    public void PlayLanding()
    {
        if (seSource != null && landingSE != null)
        {
            seSource.PlayOneShot(landingSE);
        }
    }
}