using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;

    public AudioClip correctClip;
    public AudioClip incorrectClip;

    private void Awake()
    {
        instance = this;
    }

    public void PlayCorrect()
    {
        if (correctClip != null)
        {
            audioSource.PlayOneShot(correctClip);
        }
    }

    public void PlayIncorrect()
    {
        if (incorrectClip != null)
        {
            audioSource.PlayOneShot(incorrectClip, 0.25f);
        }
    }
}
