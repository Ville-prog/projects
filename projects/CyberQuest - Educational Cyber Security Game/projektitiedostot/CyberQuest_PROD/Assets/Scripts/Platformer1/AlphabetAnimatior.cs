using UnityEngine;
using TMPro;

public class AlphabetAnimator : MonoBehaviour
{
    public TextMeshProUGUI tmpText; // Reference to TMP Text
    public AnimationCurve fontSizeCurve; // The curve that controls the font size
    public float animationDuration = 2f; // Duration of the animation

    private float timeElapsed = 0f;
    private bool isAnimating = false;

    void Start()
    {
        // Optionally, set an initial font size if you need
       
    }

    void Update()
    {
        if (isAnimating)
        {
            // Update the time elapsed
            timeElapsed += Time.deltaTime;

            // Normalize the time value to be between 0 and 1 based on the animation duration
            float normalizedTime = Mathf.Clamp01(timeElapsed / animationDuration);

            // Evaluate the curve at the current time
            tmpText.fontSize = fontSizeCurve.Evaluate(normalizedTime);

            // If the animation is complete, stop it
            if (normalizedTime >= 1f)
            {
                isAnimating = false;
            }
        }
    }

    // Method to start the font size animation
    public void StartFontSizeAnimation()
    {
        timeElapsed = 0f;
        isAnimating = true;
    }
}
