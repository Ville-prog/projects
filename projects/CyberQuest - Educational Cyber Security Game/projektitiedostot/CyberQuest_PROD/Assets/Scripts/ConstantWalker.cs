/*
 * ConstantWalker.cs
 * This script moves a UI element (like a character) across the screen in a constant loop.
 * Authors: Salla Valio and Meri Välimäki
 */

using UnityEngine;
using UnityEngine.UI;

public class UIWalker : MonoBehaviour
{
    public float speed = 500f; // UI speed (pixels per second)
    public float startDelay = 1f; // Time to wait before starting
    public RectTransform mainMenuCanvas; // Drag MainMenuCanvas here
    private RectTransform rectTransform;

    private Vector2 startPos;
    private Vector2 endPos;

    private float delayTimer;
    private bool canWalk = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        delayTimer = startDelay;
        // Make sure the canvas reference is set
        if (mainMenuCanvas == null)
        {
            Debug.LogError("MainMenuCanvas reference is missing!");
            return;
        }

        float canvasWidth = mainMenuCanvas.rect.width;
        float characterWidth = rectTransform.rect.width;

        // Calculate off-screen starting position (left side of canvas)
        startPos = new Vector2(-characterWidth-canvasWidth/2, rectTransform.anchoredPosition.y);

        // Calculate off-screen ending position (right side of canvas)
        endPos = new Vector2(canvasWidth/2 + characterWidth, rectTransform.anchoredPosition.y);

        rectTransform.anchoredPosition = startPos;
    }

    void Update()
    {
        // Wait for the initial delay before starting movement
        if (!canWalk)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0f)
                canWalk = true;
            else
                return;
        }
        // Move the object toward the end position
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            endPos,
            speed * Time.deltaTime
        );
        // Once the object reaches the end, loop it back to the start
        if (Vector2.Distance(rectTransform.anchoredPosition, endPos) < 1f)
        {
            rectTransform.anchoredPosition = startPos; // Loop
        }
    }
}