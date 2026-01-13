using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    [SerializeField] 
    private Canvas[] canvases;
    [SerializeField] private Button nextButton;
    [SerializeField] private LevelLoader levelLoader;

    private int canvasnumber = 0;

    void Start()
    {
        canvasnumber = 0;
        canvases[0].gameObject.SetActive(true);
        canvases[1].gameObject.SetActive(false);
        canvases[2].gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCanvas(){

        if (canvasnumber < canvases.Length - 1)
        {
            // Hide the current canvas
            canvases[canvasnumber].gameObject.SetActive(false);

            // Show the next canvas
            canvasnumber++;
            canvases[canvasnumber].gameObject.SetActive(true);
        }
        else
        {
            // Load the next scene
            LevelLoader.LoadSceneStatic("platfromer2");
            
        }
    }
}
