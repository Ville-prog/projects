using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private GameOver gameOverScript;
    public int maxHealth = 4;
    public int currentHealth;

    public HealthBar healthBar;

    public float invincibilityduration = 1.0f;  // time in seconds when player is invincible
    private float lastDamageTime = -1f; // time from last damage taken

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        if(healthBar != null){
          healthBar.SetMaxHealth(maxHealth);
        }else{
          Debug.Log("HealthBar refrence is missing");
        }
        
    }

    void Update()
    {
      healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage){
      Debug.Log("PLAYER STARTED TAKING DAMAGE");
      if(Time.time >= lastDamageTime + invincibilityduration){
        currentHealth = Mathf.Max(0, currentHealth - damage);
        healthBar.SetHealth(currentHealth);

        lastDamageTime = Time.time;
        Debug.Log("Player took damage. Health now:" + currentHealth);
      }


      if(currentHealth == 0){
        Debug.Log("you have died!!");
        // TODO transition to death screen here / last checkpoint
        gameOverScript.EndMenu();
      }
      
    }
}
