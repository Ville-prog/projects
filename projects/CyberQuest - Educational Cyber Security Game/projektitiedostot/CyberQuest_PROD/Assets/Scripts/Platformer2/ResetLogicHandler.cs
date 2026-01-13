using UnityEngine;

public class ResetLogicHandler : MonoBehaviour
{
    private bool isPlayerInRange = false;
    
    [SerializeField]
    private ButtonHandler buttonhandler; // Tracks if the player is in range of the button

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {   
       

        // If the player is near and presses the "F" key, try to begin cleanse
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            
            
                // Handle left shift
                Debug.Log("Player pressed F - Trying to Cleanse");
                buttonhandler.OnCleanseButtonClick();
                

      
        }

        
    }

    // Triggered when a collider enters the trigger area
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
