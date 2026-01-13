using UnityEngine;
using TMPro;
using System;

public class Bubble : MonoBehaviour
{
    public string origin;
    public TextMeshPro textMesh;
    public bool isCorrectOrigin;
    public GameObject onCollectEffect;

    // Helper function to split url for readability
    private string FormatUrl(string url)
    {
        try
        {
            Uri uri = new Uri(url);
            string formatted = $"{uri.Scheme}://\n{uri.Host}:{uri.Port}";

            if (!string.IsNullOrEmpty(uri.AbsolutePath) && uri.AbsolutePath != "/")
            {
                formatted += $"\n{uri.AbsolutePath}";
            }

            return formatted;
        }
        catch
        {
            return url;
        }
    }

    // Sets the bubble's origin data and updates the displayed text
    public void SetBubbleData(string newOrigin, bool isCorrect)
    {
        origin = newOrigin;
        isCorrectOrigin = isCorrect;

        string formattedUrl = FormatUrl(origin);
        textMesh.text = formattedUrl;
    }

    // Destroys bubbles while colliding on player or ground, reduces player health on incorrect origins
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isCorrectOrigin)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
                SoundManager.instance.PlayIncorrect();
            }
            else
            {
                SoundManager.instance.PlayCorrect();
            }

                Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
