/*
 * CharacterColorChange.cs
 * This script handles the color change of a character sprite in Unity. 
 * Authors: Salla Valio and Meri Välimäki
 */

using UnityEngine;
using UnityEngine.UI; // For UI button handling
 
public class CharacterColorChange : MonoBehaviour
{
    public SpriteRenderer jamppaSprite; // Reference to Jamppa's SpriteRenderer
 
    public Sprite[] colorSprites; // Array to hold the individual sprites for each color option
 
    void Start()
    {
        // Initialize the color sprites by dragging them into the Inspector
        // colorSprites[0] should correspond to Hahmovarit_0, colorSprites[1] to Hahmovarit_1, etc.
 
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
    }
 
    // Update the character's sprite with the selected color
    public void UpdateCharacterColor()
    {
        if (jamppaSprite != null && colorSprites.Length > 0)
        {
            jamppaSprite.sprite = colorSprites[SwitchCharacter.currentColorIndex]; // Update sprite to the current color
        }
    }
 
}
 