using UnityEngine;

public class PlayerStamina : MonoBehaviour
{

    public int maxStamina = 100;
    public int currentStamina;

    public StaminaBar staminaBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)){
          TakeExhaust(5);
        }
    }

    void TakeExhaust(int exhaust){
      
      currentStamina -= exhaust;
      staminaBar.SetStamina(currentStamina);
    }
}
