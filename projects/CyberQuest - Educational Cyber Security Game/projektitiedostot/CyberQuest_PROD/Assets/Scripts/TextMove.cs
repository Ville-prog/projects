/*
 * TextMove.cs 
 * This script moves a text element upwards and snaps it back to the center of the screen.
 * It also manages the visibility of a scrollbar and a continue button.
 * Authors: Salla Valio and Meri Välimäki
 */

using UnityEngine;
using UnityEngine.UI;
 
public class TextMove : MonoBehaviour
{
    public RectTransform textRect;
    public Vector2 moveUpTarget = new Vector2(0, 800); // Target position when going up
    public Vector2 centerPosition = new Vector2(0, 0); // Where it reappears
    public float moveSpeed = 300f;
    public float delayBeforeSnap = 0.5f;
    public float delayBeforeStart = 3f;
 
    private bool movingUp = true;
    private bool hasSnapped = false;
    private float startTime;
    // UI references 
    public Button continueButton;
    public float scrollbarWidth = 10f;
    public ScrollRect scrollRect;
    public RectTransform content;
    public Scrollbar scrollBar;


 
    void Start()
    {
        startTime = Time.time;
        // Hide the scrollbar and its background image initially
        if (scrollRect.verticalScrollbar != null)
        {
            // Hide the handle
            scrollRect.verticalScrollbar.handleRect.gameObject.SetActive(false);

            // Hide the background image (the track)
            Image bgImage = scrollRect.verticalScrollbar.GetComponent<Image>();
            if (bgImage != null)
                bgImage.enabled = false;
        }


        if (textRect == null)
            textRect = GetComponent<RectTransform>();

        // Force layout rebuild to get proper content height
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        textRect.anchoredPosition = new Vector2(centerPosition.x, -100);
        
    }
 
    void Update()
    {
        
        if (Time.time - startTime < delayBeforeStart)
        {
            return;
        }

        if (movingUp)
        {
            // Move the text towards the target position
            textRect.anchoredPosition = Vector2.MoveTowards(textRect.anchoredPosition, moveUpTarget, moveSpeed * Time.deltaTime);
 
            if (Vector2.Distance(textRect.anchoredPosition, moveUpTarget) < 0.1f)
            {
                movingUp = false;
                Invoke(nameof(SnapToCenter), delayBeforeSnap);
            }
            //Debug.Log("Pos: " + textRect.anchoredPosition);
        }
    }
    // Snaps the text back to the center and enables additional UI
    void SnapToCenter()
    {
        if (!hasSnapped)
        {
            textRect.anchoredPosition = centerPosition;
            hasSnapped = true;
            //scrollbar is visible
            if (scrollRect.verticalScrollbar != null)
            {
                scrollRect.verticalScrollbar.handleRect.gameObject.SetActive(true);

                Image bgImage = scrollRect.verticalScrollbar.GetComponent<Image>();
                if (bgImage != null)
                    bgImage.enabled = true;
            }

            RectTransform contentRect = content.GetComponent<RectTransform>();
            float contentHeight = contentRect.rect.height;

            if (scrollRect.verticalScrollbar != null)
            {
                // Let Unity handle the scrollbar handle size automatically
                // Make sure content height is updated if necessary
                LayoutRebuilder.ForceRebuildLayoutImmediate(content);
 
                // Optional: set width of scrollbar itself (not the handle)
                scrollRect.verticalScrollbar.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollbarWidth, scrollRect.verticalScrollbar.GetComponent<RectTransform>().sizeDelta.y);
            }

            // Optional: change font size for better readability
            Text textComponent = textRect.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.fontSize = 20; // Adjust this value as needed
            }
 
             // Make sure the ScrollRect starts at the top
            scrollRect.verticalNormalizedPosition = 1f;
                

        }
    }
}