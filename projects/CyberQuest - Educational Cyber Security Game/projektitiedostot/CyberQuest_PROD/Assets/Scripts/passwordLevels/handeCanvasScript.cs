using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class handeCanvasScript : MonoBehaviour
{
    [SerializeField] private Canvas canvas1;
    [SerializeField] private Canvas canvas2;
    [SerializeField] private Canvas canvas3;
    
    [SerializeField] private Button testSkillsButton;

    void Start()
    {
        // Show canvas1
        canvas1.gameObject.SetActive(true);

        // Hide canvas2 and canvas3
        canvas2.gameObject.SetActive(false);
        canvas3.gameObject.SetActive(false);
    }

    public void StartTutorial()
    {
        // Hide canvas1 and canvas3
        canvas1.gameObject.SetActive(false);
        canvas3.gameObject.SetActive(false);

        // Show canvas2
        canvas2.gameObject.SetActive(true);

        // Hide the button
        testSkillsButton.gameObject.SetActive(false);
    }

    public void PutSkillsToTest()
    {
        // Hide canvas1 and canvas2
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(false);

        // Show canvas3
        canvas3.gameObject.SetActive(true);
    }

    public void ShowSkillsToTestButton()
    {
        // Show the button
        testSkillsButton.gameObject.SetActive(true);
    }

    public void ShowFinalCanvas()
    {
        // Hide canvas2 and canvas3
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(false);

        // Show canvas3
        canvas3.gameObject.SetActive(true);
    } 
}
