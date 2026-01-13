using UnityEngine;

// Monitors elapsed time and player health, sets winning screen
public class LevelTimer : MonoBehaviour
{
    public float levelDuration = 60f;
    private float timeRemaining;
    private bool levelComplete = false;

    public PlayerHealth playerHealth;
    public CanvasGroup winScreen;
    public CanvasGroupFader fader;

    void Start()
    {
        timeRemaining = levelDuration;
    }

    void Update()
    {
        if (levelComplete || playerHealth.currentHealth <= 0) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            levelComplete = true;
            StartCoroutine(fader.FadeIn(winScreen, 1f));
            Time.timeScale = 0f;
            
        }
    }
}
