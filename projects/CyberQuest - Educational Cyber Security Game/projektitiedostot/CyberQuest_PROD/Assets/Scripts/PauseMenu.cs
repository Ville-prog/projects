using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject quitButton;
    private LevelLoader levelLoader;

    void Start(){
      levelLoader = FindFirstObjectByType<LevelLoader>();
      if(levelLoader == null){
        Debug.LogError("PauseMenu: LevelLoader not found in the scene");
      }

      #if UNITY_WEBGL
        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }
      #endif
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
          if(GameIsPaused){
            Debug.Log("RESUME");
            Resume();
          }else{
            Debug.Log("PAUSED");
            Pause();
          }
        }
    }

    public void Resume(){
      pauseMenuUI.SetActive(false);
      Time.timeScale = 1f;
      GameIsPaused = false;
    }

    void Pause(){
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0f;
      GameIsPaused = true;
    }

    public void MainMenu(){
      GameIsPaused = false;
      Time.timeScale = 1f;
    }

    public void LevelMenu(){
        GameIsPaused = false;
        Time.timeScale = 1f;
    }


    public void quitGame(){
      Debug.Log("Quitting Game");
      #if UNITY_WEBGL
        //Do nothing on web build
      #else
        Application.Quit();
      #endif
    }
}
