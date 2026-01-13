using Unity.VisualScripting;
using UnityEngine;

public class ButtonPuzzleLogic : MonoBehaviour
{   
    // References to other components
    public GameObject puzzleScreen;
    public CaesarScript script;

    private Renderer screenRenderer; // For changing the button color
    private bool isPlayerInRange = false; // Tracks if the player is in range of the button
    private bool isLeftButton; // Tracks if this button is for shifting left (vs. right)

    private bool puzzleSolved = false;

    public float buttonCoolDown;

    private float cooldownTimer = 1f;

    // Start is called once before the first frame update
    void Start()
    {
        // Get the renderer of the puzzle screen to change its color
        screenRenderer = puzzleScreen.GetComponent<Renderer>();

        // Check if this button is a LeftShiftButton or RightShiftButton
        isLeftButton = gameObject.CompareTag("LeftShiftButton");
    }

    // Update is called once per frame
    void Update()
    {   
       

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime; // Decrease the cooldown timer each frame
        }

        //check if puzzle is solved first

        puzzleSolved = script.getSolvedorNot();

        

        if (puzzleSolved){

            return;
        }
        // If the player is near and presses the "F" key, shift accordingly
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && cooldownTimer <= 0)
        {
            if (isLeftButton)
            {
                // Handle left shift
                Debug.Log("Player pressed F - Shifting Left");
                script.ShiftLeft();
                cooldownTimer = buttonCoolDown;
                
                
            }
            else
            {
                // Handle right shift
                Debug.Log("Player pressed F - Shifting Right");
                script.ShiftRight();
                cooldownTimer = buttonCoolDown;
               
            }

             Debug.Log("Timer: " + cooldownTimer);

      
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

    // Function to set a random color for the button
    
}
