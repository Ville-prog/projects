using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class timeSurvidedTimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private LevelLoader levelLoader;

    private float timeRemaining = 120; // 2 minutes in seconds
    private bool isCountingDown = true;

    void Start()
    {    
        levelLoader = Object.FindFirstObjectByType<LevelLoader>();        
    }

    void Update()
    {
        
        if (isCountingDown && timeRemaining > 0)
        {
            // Decrease the remaining time
            timeRemaining -= Time.deltaTime;

            // Clamp the time to avoid negative values
            timeRemaining = Mathf.Max(timeRemaining, 0);

            // Update the timer text
            UpdateTimerText();
        }
        else if (timeRemaining <= 0 && isCountingDown)
        {
            isCountingDown = false;
            
            // Load the next scene index dynamically
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            levelLoader.LoadLevel(nextSceneIndex);
        }
    }

    private void UpdateTimerText()
    {
        // Convert time to minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Update the TextMeshProUGUI text
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
