using UnityEngine;
using UnityEngine.SceneManagement;

public class pwGameScore : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private LevelLoader levelLoader;
    [SerializeField] public GameOver gameOverScript;
    
    private bool gameIsOver = false;

    void Start()
    {
        levelLoader = Object.FindFirstObjectByType<LevelLoader>();
    }

    void Update()
    {
        if (healthBar.slider.value <= 0 & !gameIsOver) 
        {
            gameOver();
        }
    }

    private void gameOver() 
    {
        gameIsOver = true;
        gameOverScript.EndMenu();
    }
}
