using UnityEngine;

public class SecurityCameraSoundsScript : MonoBehaviour
{
    public AudioClip detectSE;   // プレイヤー発見音
    public AudioClip alarmSE;    // 警報音

    private AudioSource audioSource;

    private bool isAlarm;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // プレイヤーを発見した時
    public void PlayDetectSound()
    {
        audioSource.PlayOneShot(detectSE);
    }

    // 警報開始
    public void StartAlarm()
    {
        if (isAlarm)
            return;

        isAlarm = true;

        audioSource.clip = alarmSE;
        audioSource.loop = true;
        audioSource.Play();
    }

    // 警報停止
    public void StopAlarm()
    {
        if (!isAlarm)
            return;

        isAlarm = false;

        audioSource.Stop();
    }
}
