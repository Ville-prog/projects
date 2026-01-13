using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class passwordScript : MonoBehaviour
{
    [SerializeField] private GameObject spaceBug;
    [SerializeField] private TextMeshProUGUI passwordText;
    [SerializeField] private AudioSource audioSource;
    private HealthBar healthBar;

    private string password; // The randomly selected password
    private int tier;        // The tier of the password

    public void Initialize(Dictionary<int, List<string>> passwordsByTier, int tier)
    {
        this.tier = tier;

        // Pull a random password from the specified tier
        if (passwordsByTier.ContainsKey(tier) && passwordsByTier[tier].Count > 0)
        {
            int randomIndex = Random.Range(0, passwordsByTier[tier].Count);
            password = passwordsByTier[tier][randomIndex];
        }
       
        // Set the TextMeshPro text to the selected password
        if (passwordText != null)
        {
            passwordText.text = password;
        }
    }

    void Start()
    { 
        GameObject healthBarObject = GameObject.Find("HealthBar");
        if (healthBarObject != null)
        {
            healthBar = healthBarObject.GetComponent<HealthBar>();
        }

        // Randomize the color of the spaceBug
        SpriteRenderer spriteRenderer = spaceBug.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(
            Random.Range(0f, 1f), // Random red value
            Random.Range(0f, 1f), // Random green value
            Random.Range(0f, 1f)  // Random blue value
        );    
    }

    void Update()
    {
        // Rotate the spaceBug back and forth
        float angle = Mathf.Sin(Time.time * 2) * 7;
        spaceBug.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Destroy this GameObject if it goes below y = -7
        if (transform.position.y <= -6f)
        {
            Debug.Log($"Password Object Destroyed - Password: {password}, Tier: {tier}");

            // Reduce health based on the tier
            if (healthBar != null)
            {
                int healthReduction = 0;

                // Determine health reduction based on the tier
                switch (tier)
                {
                    case 0:
                        healthReduction = 2; // Tier 0 reduces 2 health points
                        break;

                    case 1:
                        healthReduction = 1; // Tier 1 reduces 1 health point
                        break;

                    // Tier 3 and 4 don't reduce hp
                    default:
                        healthReduction = 0;
                        break;
                }
                // Apply the health reduction
                healthBar.SetHealth((int)healthBar.slider.value - healthReduction);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "Laser" tag
        if (collision.CompareTag("Laser"))
        {

            // Reduce health if destroyed good or bery good passwords
            if (healthBar != null)
            {
                int healthReduction = 0;
                switch (tier)
                {
                    case 2:
                        healthReduction = 1; // Tier 2 reduces 1 health point
                        break;

                    case 3:
                        healthReduction = 2; // Tier 3 reduces 2 health points
                        break;

                    default:
                        healthReduction = 0; // Other tiers don't reduce health
                        break;
                }

                // Apply the health reduction
                healthBar.SetHealth((int)healthBar.slider.value - healthReduction);
            }
    
                
             // Create a temporary GameObject to play the audio
            if (audioSource != null && audioSource.clip != null)
            {
                GameObject tempAudioObject = new GameObject("TempAudio");
                AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();

                // Copy the properties of the original AudioSource
                tempAudioSource.clip = audioSource.clip;
                tempAudioSource.volume = audioSource.volume;
                tempAudioSource.pitch = audioSource.pitch;
               
                // Play the audio and destroy the temporary GameObject after the clip finishes
                tempAudioSource.Play();
                Destroy(tempAudioObject, audioSource.clip.length);
            }
            

            // Destroy this GameObject (passwordObject)
            Destroy(gameObject);

            // Destroy the laser clone
            Destroy(collision.gameObject);
        }
    }
}
