using UnityEngine;
using UnityEngine.UI;
using TarodevController; 

/*
 * This class is responsible for updating the UI elements HP text and score text.
 * It retrieves the player's HP and updates the corresponding UI text. It also
 * calculates and updates the score based on the elapsed time since the game
 * started.
 */
public class UIScript : MonoBehaviour
{
    // "[SerializeField]" allows private fields to be visible and editable in
    // the Unity Inspector.
    [SerializeField] private Text Hp_text;
    [SerializeField] private Text Score_text;

    private TarodevController.PlayerController player; // player gameObject
    private float score;
    private float startTime;

    /*
     * Start is called before the first frame update
     */ 
    void Start()
    {
        // Find the player GameObject
        player = FindObjectOfType<TarodevController.PlayerController>();

        startTime = Time.time;
    }

    /*
     * Update is called once per frame
     */
    void Update()
    {   
        Hp_text.text = $"HP: {player.getHp()}";

        // Calculate score
        score = Time.time - startTime;
        int minutes = Mathf.FloorToInt(score / 60F);
        int seconds = Mathf.FloorToInt(score % 60F);

         // Update the Score while player is alive
        if (player.getHp() > 0) 
        {
            if (minutes > 0)
            {
                Score_text.text = $"Time survived: {minutes:00}:{seconds:00}";
            }
            else
            {
                Score_text.text = $"Time survived: {seconds:0}s";
            } 
        }
    }
}