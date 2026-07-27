using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource footstepSource; // 足音用
    public AudioSource seSource;       // ジャンプ・着地・ダッシュ用

    [Header("Sound Effects")]
    public AudioClip footstepSE;
    public AudioClip jumpSE;
    public AudioClip landingSE;
    public AudioClip dashSE;
    private bool isDashPlaying = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (footstepSource == null)
        {
            footstepSource = GetComponents<AudioSource>()[0];
        }

        if (seSource == null)
        {
            seSource = GetComponents<AudioSource>()[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 足音
    public void PlayFootstep()
    {
        footstepSource.clip = footstepSE;
        footstepSource.loop = true;

        if (!footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
    }

    public void StopFootstep()
    {
        footstepSource.Stop();
    }

    // ジャンプ
    public void PlayJump()
    {
        seSource.PlayOneShot(jumpSE);
    }

    //着地
    public void PlayLanding()
    {
        seSource.PlayOneShot(landingSE);
    }

    // ダッシュ
    public void PlayDash()
    {
        if (!isDashPlaying)
        {
            seSource.PlayOneShot(dashSE);
            isDashPlaying = true;
        }
    }

    public void StopDash()
    {
        isDashPlaying = false;
    }
}
