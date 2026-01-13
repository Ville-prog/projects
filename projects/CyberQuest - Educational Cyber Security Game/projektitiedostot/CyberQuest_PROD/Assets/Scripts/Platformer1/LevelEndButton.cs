using UnityEngine;

public class LevelEndButton : MonoBehaviour
{   

    [SerializeField]
    private LevelLoader levelLoader;
    public bool isPlayerInRange = false; // Flag to check if the player is in range of the button
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            
            Debug.Log("Ending Level - Player pressed F");
            
            
            levelLoader.LoadLevel("computerDesktopScene"); // End Level 

                //TODO!!! 
                
            }
        
    }


private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player entered the range of the button
            isPlayerInRange = true;
            
        }
    }

    // Triggered when a collider exits the trigger area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player left the range of the button
            isPlayerInRange = false;
        }
    }

 }


