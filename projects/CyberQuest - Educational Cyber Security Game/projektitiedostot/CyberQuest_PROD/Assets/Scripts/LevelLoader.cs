using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
  // static object
  public static LevelLoader Instance { get; private set; }
  [SerializeField] Animator transitionAnim;

  public void Awake()
  {
    if(Instance == null){
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }else{
      Destroy(gameObject);  // destroys other copies if any
    }
    gameObject.SetActive(true);
  }

  // ANY SCENE
  // used for abnormal scene changes (mainmenu to settings and back, level selector)
  public void LoadLevel(int sceneIndex){
    StartCoroutine(LoadScene(sceneIndex));
  }
  public void LoadLevel(string sceneName){
    Time.timeScale = 1f;
    StartCoroutine(LoadScene(sceneName));
  }

  // NEXT LEVEL CHANGES
  // remember to keep the scene build ids in order
  public void NextLevel(){
    Debug.Log("NextLevel");
    Instance.StartCoroutine(Instance.LoadScene(SceneManager.GetActiveScene().buildIndex+1));
  }

  // A static method index AND scene name methods
  public static void LoadSceneStatic(int sceneIndex)
  {
    if (Instance != null)
    {
        Instance.LoadLevel(sceneIndex);
    }
    else
    {
        Debug.LogError("LevelLoader instance not found!");
    }
  }
  public static void LoadSceneStatic(string sceneName)
  {
      if (Instance != null)
      {
          Instance.LoadLevel(sceneName);
      }
      else
      {
          Debug.LogError("LevelLoader instance not found!");
      }
  }

  // Coroutine that loads the scene
  private IEnumerator LoadScene(object SceneIdentifier){
    Debug.Log(gameObject.activeInHierarchy);
    gameObject.SetActive(true);
    transitionAnim.SetTrigger("End");
    yield return new WaitForSeconds(1);
    if(SceneIdentifier is int sceneIndex){
      SceneManager.LoadSceneAsync(sceneIndex);
    }else if(SceneIdentifier is string sceneName){
      SceneManager.LoadSceneAsync(sceneName);
    }else{
      Debug.LogError("Invalid scene type. Must be string or int");
    }
    transitionAnim.SetTrigger("Start");
  }
}
