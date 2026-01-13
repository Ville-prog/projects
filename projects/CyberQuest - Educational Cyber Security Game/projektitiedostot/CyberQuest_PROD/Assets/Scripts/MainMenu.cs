using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 1f;
    public GameObject quitButton;

    private void Start()
    {
        //Hide quit button on web build
        #if UNITY_WEBGL
            if (quitButton != null)
            {
                quitButton.SetActive(false);
            }
        #endif
    }

    public void QuitGame(){
      Debug.Log("quitting");
      #if UNITY_WEBGL
        //No quit action on web build
      #else
        Application.Quit();
      #endif
    }

}
