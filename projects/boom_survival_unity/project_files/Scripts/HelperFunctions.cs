using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/*
 * This class contains various useful functions needed troughout
 * the game's runtime. Also handles game ending logic and scene transitions.
 */ 
public class HelperFunctions : MonoBehaviour
{
    // "[SerializeField]" allows private fields to be visible and editable in
    // the Unity Inspector.
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera gameOverCamera;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text finalScoreLabel;
    [SerializeField] private BlockScript blockScript;

    private TarodevController.PlayerController player; // player gameObject
    
    /*
     * Start is called before the first frame update
     */ 
    void Start()
    {
        player = FindObjectOfType<TarodevController.PlayerController>();

        gameOverCamera.transform.position = new Vector3(
            (blockScript.getGridSize() / 2) -1, 150, -260);
        gameOverCamera.enabled = false;
    }

    /*
     * Update is called once per frame
     */
    void Update() 
    {
        if (player.getHp() <= 0) {
            GameOver();
        }
    }

    /*
     * Calculates distance between two gameObjects and returns a boolean value
     * based on wether they are within a certain distance of eachother.
     */
    public static bool IsObjectNearPlayer(GameObject obj, GameObject obj2,
     float threshold = 10f)
    {
        if (obj == null || obj2 == null) return false;

        float distance = Vector3.Distance(obj2.transform.position,
         obj.transform.position);

        bool isNear = distance <= threshold;
        return isNear;
    }

    void GameOver()
    {
        setFinalScoreLabel();
        Destroy(player.gameObject);
        StartCoroutine(CameraTransition());
    }

    /*
     * Transitions view smoothly from maincamera to gameovercamera
     */ 
    IEnumerator CameraTransition()
    {
        uiCanvas.enabled = false;
        gameOverCamera.GetComponent<AudioListener>().enabled = true;

        float duration = 3.0f;
        float elapsedTime = 0.0f;

        Vector3 startingPosition = mainCamera.transform.position;
        Quaternion startingRotation = mainCamera.transform.rotation;
        float startingSize = mainCamera.orthographicSize; 
        
        Vector3 targetPosition = gameOverCamera.transform.position;
        Quaternion targetRotation = gameOverCamera.transform.rotation;
        float targetSize = gameOverCamera.orthographicSize * 30; 
        float targetFOV = gameOverCamera.fieldOfView * 30; 

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            mainCamera.transform.position = Vector3.Lerp(startingPosition, targetPosition, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startingRotation, targetRotation, t);
            mainCamera.orthographicSize = Mathf.Lerp(startingSize, targetSize, t); 

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
        mainCamera.orthographicSize = targetSize; 

        gameOverCamera.enabled = true;
        mainCamera.enabled = false;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void setFinalScoreLabel()
    {
        finalScoreLabel.text = scoreLabel.text;   
    }
}

