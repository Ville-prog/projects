using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideTextController : MonoBehaviour
{
    public HighlightController highlightController;
    
    public TMP_Text guideText;
    public Button nextButton;
    public GameObject gameUI;
    public GameObject guideUI;

    private int step = 0;
    private int lastStep = 3;
    private string[] guideSteps = {
    "<b>Every website has an origin.</b>\nA website’s origin is like its home address on the internet.",
    "<b>An origin is made up of three parts:</b>\nProtocol (http or https) – How the browser talks to the website\nHost (example.com) – The website’s name\nPort (often hidden) – A technical detail for communication",
    "<b>Same-Origin Policy (SOP) keeps websites safe.</b>\nWebsites can only interact with their own origin. This prevents one site from secretly stealing data from another!",
    "<b>Now it's your turn to guard this website with SOP!</b>\nCatch requests from the same origin and avoid the strange ones."
};

    void Start()
    {
        nextButton.onClick.AddListener(NextStep);
        guideText.text = guideSteps[step];
    }
    public void NextStep()
    {
        step++;
        if (step < guideSteps.Length)
        {
            guideText.text = guideSteps[step];

            if (step == 1)
            {
                highlightController.ShowHighlights();
            }

            if (step == lastStep)
            {
                nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Begin!";
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(BeginGame);

            }
            
        }
        
    }
    private void BeginGame ()
    {
        guideUI.SetActive(false);
        gameUI.SetActive(true);
    }
}
