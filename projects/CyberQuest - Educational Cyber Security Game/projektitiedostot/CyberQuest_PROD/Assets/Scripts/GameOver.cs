using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
  public GameObject DeathScreen;
  private LevelLoader levelLoader;


  void Start()
  {
      DeathScreen.SetActive(false);
  }
  public void EndMenu (){
    DeathScreen.SetActive(true);
    Time.timeScale = 0f;
  }

  public void TryAgain(){

    if (FindAnyObjectByType<PlayerHealth>()!= null){
      FindAnyObjectByType<PlayerHealth>().currentHealth = 4;
    }

    DeathScreen.SetActive(false);
    
    Time.timeScale = 1f;
    Debug.Log(SceneManager.GetActiveScene().buildIndex);
    LevelLoader.LoadSceneStatic(SceneManager.GetActiveScene().buildIndex);
  }

  public void ToMenu(){
    Debug.Log("TOMENU FROM THE DEAD");
    Time.timeScale = 1f;
    DeathScreen.SetActive(false);
    
    LevelLoader.LoadSceneStatic("MainMenu");
  }

  // Villen funktio !!! poista!!1
  public void restartEmailFromStart() {
    Time.timeScale = 1f;
    Debug.Log(SceneManager.GetActiveScene().buildIndex);
    LevelLoader.LoadSceneStatic("Email_Level");  
  }
}
