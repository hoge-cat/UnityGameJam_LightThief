using UnityEngine;

public class TitleMenuSounds : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip moveSE;
    [SerializeField] private AudioClip decideSE;

    public void PlayMove()
    {
        PlaySound(moveSE);
    }

    public void PlayDecide()
    {
        PlaySound(decideSE);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.Stop();
        audioSource.PlayOneShot(clip);
    }
}