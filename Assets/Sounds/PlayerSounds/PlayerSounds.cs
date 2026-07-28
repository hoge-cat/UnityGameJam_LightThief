using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource footstepSource; // 足音用
    public AudioSource dashSource;       // ジャンプ・着地・ダッシュ用
    public AudioSource seSource;       // ジャンプ・着地
    private float dashTimer = 0f;
    [SerializeField] private float dashInterval = 0.25f;

    [Header("Sound Effects")]
    public AudioClip footstepSE;
    public AudioClip jumpSE;
    public AudioClip landingSE;
    public AudioClip dashSE;
    private bool isDashPlaying = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (footstepSource == null && sources.Length > 0)
            footstepSource = sources[0];

        if (dashSource == null && sources.Length > 1)
            dashSource = sources[1];

        if (seSource == null && sources.Length > 2)
            seSource = sources[2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 足音
    public void PlayFootstep()
    {
        if (!footstepSource.isPlaying)
        {
            footstepSource.clip = footstepSE;
            footstepSource.loop = false;
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
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            dashTimer = dashInterval;
            dashSource.PlayOneShot(dashSE);
        }
    }

    public void StopDash()
    {
        dashTimer = 0f;
    }
}
