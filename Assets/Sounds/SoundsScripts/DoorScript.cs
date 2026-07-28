using UnityEngine;

public class DoorSound : MonoBehaviour
{
    public AudioSource audioSource;
    public DoorSound doorSound;

    public AudioClip openSE;
    public AudioClip closeSE;

    public void PlayOpenSound()
    {
        audioSource.PlayOneShot(openSE);
    }

    public void PlayCloseSound()
    {
        audioSource.PlayOneShot(closeSE);
    }
}
