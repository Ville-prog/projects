using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class pwTutorialScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private handeCanvasScript canvasHandler;

    private int stage = 0;

    void Start()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(OnInputChanged);
            inputField.ActivateInputField();
        }
    }

    private void OnInputChanged(string input)
    {
        int previousStage = stage;
        stage = EvaluateStage(input); // Check valid stage

        if (stage != previousStage)
        {
            UpdateInstruction(stage);
        }

        if (stage == 5)
        {
            tutorialText.text = "Well done Jamppa! You now know how to build a strong password.";
            inputField.interactable = false;
            canvasHandler.ShowSkillsToTestButton();
        }

        inputField.ActivateInputField();
    }

    private int EvaluateStage(string input)
    {
        if (!IsValidInputStage1(input)) return 0;
        if (!IsValidInputStage2(input)) return 1;
        if (!IsValidInputStage3(input)) return 2;
        if (!IsValidInputStage4(input)) return 3;
        if (input.Length < 12) return 4;
        return 5;
    }

    private void UpdateInstruction(int currentStage)
    {
        switch (currentStage)
        {
            case 0:
                tutorialText.text = "Start with your favorite animal. Use at least 3 lowercase letters.";
                break;
            case 1:
                tutorialText.text = "Very good! Now try adding a capital letter.";
                break;
            case 2:
                tutorialText.text = "Nice! Now add a number.";
                break;
            case 3:
                tutorialText.text = "Great! Now add a special character like !, @, or #.";
                break;
            case 4:
                tutorialText.text = "Excellent! Now make it longer (12+ characters).";
                break;
        }
    }

    // At least 2 lowercase letters anywhere in the input
    private bool IsValidInputStage1(string input)
    {
        return Regex.Matches(input, "[a-z]").Count >= 2;
    }

    private bool IsValidInputStage2(string input)
    {
        return Regex.IsMatch(input, "[A-Z]");
    }

    private bool IsValidInputStage3(string input)
    {
        return Regex.IsMatch(input, @"\d");
    }

    private bool IsValidInputStage4(string input)
    {
        return Regex.IsMatch(input, @"[!@#$%^&*(),.?""':{}|<>]");
    }
}
