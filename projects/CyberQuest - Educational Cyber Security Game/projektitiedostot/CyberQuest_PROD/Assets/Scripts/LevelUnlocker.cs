/*
 * LevelUnlocker.cs
 * This script unlocks levels in a game based on the last level completed.
 * Authors: Salla Valio and Meri Välimäki
 */
 
using UnityEngine;
 
public class LevelUnlocker : MonoBehaviour
{
    //nappeja jota unlockataan
    public GameObject level2Button;
    public GameObject level3Button;
    public GameObject level4Button;
    public GameObject level5Button;
    public GameObject level2image;
    public GameObject level3image;
    public GameObject level4image;
    public GameObject level5image;
 
    public int lastLevelCompleted;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //asetetaan napit näkymättömiksi tai niiku pois käytöstä
        level2Button.SetActive(false);
        level3Button.SetActive(false);
        level4Button.SetActive(false);
        level5Button.SetActive(false);
        level2image.SetActive(true);
        level3image.SetActive(true);
        level4image.SetActive(true);
        level5image.SetActive(true);
        
 
        UnlockLevel();
 
    }
 
    // Update is called once per frame
    void Update()
    {
       
    }
   
    //avaa tason
    void UnlockLevel()
    {
        if (lastLevelCompleted >= 1){
            level2Button.SetActive(true);
            level2image.SetActive(false);
        }
        if (lastLevelCompleted >= 2){
            level3Button.SetActive(true);
            level3image.SetActive(false);
        }
        if (lastLevelCompleted >= 3){
            level4Button.SetActive(true);
            level4image.SetActive(false);
        }
        if (lastLevelCompleted >= 4){
            level5Button.SetActive(true);
            level5image.SetActive(false);
        }
        /*default:
            Debug.Log("No more levels to unlock or unknown level.");
            break;
        }*/
    }
}
 
