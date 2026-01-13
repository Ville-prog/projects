/*
 * JamppaColorSelector.cs
 * This script allows the player to cycle through different color options for the character "Jamppa"
 * using arrow buttons in the UI. The selected color is applied to the character's sprite.
 * Authors: Salla Valio and Meri Välimäki
 */

using UnityEngine;
using UnityEngine.UI; // For UI button handling
 
public class JamppaColorSelector : MonoBehaviour
{
    public Image jamppaSprite; // Reference to Jamppa's SpriteRenderer
    public Button leftArrowButton; // Button for scrolling left (previous color)
    public Button rightArrowButton; // Button for scrolling right (next color)
 
    public Sprite[] colorSprites; // Array to hold the individual sprites for each color option
 
    void Start()
    {
        // Initialize the color sprites by dragging them into the Inspector
        // colorSprites[0] should correspond to Hahmovarit_0, colorSprites[1] to Hahmovarit_1, etc.
 
        // Add listeners to the arrow buttons to cycle through the colors
        leftArrowButton.onClick.AddListener(ScrollLeft);
        rightArrowButton.onClick.AddListener(ScrollRight);
 
        // Initialize with the first color
        UpdateCharacterColor();
    }
 
    // Method to scroll left (previous color)
    public void ScrollLeft()
    {
        Debug.Log("Button was clicked!");
        // Decrease the current color index and wrap around if needed
        SwitchCharacter.currentColorIndex--;
 
        if (SwitchCharacter.currentColorIndex < 0)
        {
            SwitchCharacter.currentColorIndex = colorSprites.Length - 1; // Wrap around to the last color
        }
 
        UpdateCharacterColor();
        UpdateCharacterAnimator();
    }
 
    // Method to scroll right (next color)
    public void ScrollRight()
    {
        // Increase the current color index and wrap around if needed
        SwitchCharacter.currentColorIndex++;
 
        if (SwitchCharacter.currentColorIndex >= colorSprites.Length)
        {
            SwitchCharacter.currentColorIndex = 0; // Wrap around to the first color
        }
 
        UpdateCharacterColor();
        UpdateCharacterAnimator();
    }
 
    // Update the character's sprite with the selected color
    public void UpdateCharacterColor()
    {
        if (jamppaSprite != null && colorSprites.Length > 0)
        {
            jamppaSprite.sprite = colorSprites[SwitchCharacter.currentColorIndex]; // Update sprite to the current color
        }
    }
 
    void UpdateCharacterAnimator()
{
    SwitchCharacter character = FindFirstObjectByType<SwitchCharacter>(); // Find the player object
    if (character != null)
    {
        character.UpdateAnimator(); // Call UpdateAnimator() in SwitchCharacter
    }
}
 
    void UpdateCharacterGameColor()
    {
        CharacterColorChange character = FindFirstObjectByType<CharacterColorChange>(); // Find the player object
        if (character != null)
        {
            character.UpdateCharacterColor();
        }
    }
 
}