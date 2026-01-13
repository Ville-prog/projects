using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public CanvasGroup winCanvasGroup;
    public CanvasGroupFader fader;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(fader.FadeIn(winCanvasGroup, 1f));
            Time.timeScale = 0f;
        }
    }
}
