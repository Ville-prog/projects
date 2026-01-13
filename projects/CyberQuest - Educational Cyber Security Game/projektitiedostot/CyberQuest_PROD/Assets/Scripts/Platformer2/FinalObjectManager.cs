using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

using TMPro; // Import the TextMeshPro namespace
public class FinalObjectManager : MonoBehaviour

{

    [SerializeField]
    private GameOver gameOverScript;
    [SerializeField]
    private bool infected;

    [SerializeField]
    private GameObject[] previous;

    private Renderer renderer;

   

    private Color startColor = new Color(0f, 1f, 0f), endColor = new Color(1f, 0f, 0f);

    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private TextMeshProUGUI healthText; // Reference to the TextMeshProUGUI component


    //Objects connecting to the final object, affecting it's health
    [SerializeField]
    private GameObject[] streams;

    [SerializeField]
    private float damageSpeed = 1f;

    // Multipilier for the speed of the damage taken by the final object, dependant on the amount of infected streams connected to it
    [SerializeField]
    private float infectedMultiplier = 1f;

    private Coroutine coroutine;

    private bool isInfecting;

    



    void Start(){

        currentHealth = maxHealth;
        renderer = GetComponent<Renderer>();
    }
    

    void FixedUpdate(){

        

        if (currentHealth >= 0){

            checkstreamInfection();
            healthText.text = "Health: " + currentHealth.ToString("F2"); // Update the health text

        }

        else{

           

            if (currentHealth <= 0.0f){

                gameOverScript.EndMenu();
                currentHealth = maxHealth;
            }
        }
       
    }

    void checkstreamInfection(){

        bool isanyStreamInfected = false; // Flag to check if any stream is infected
        if (streams.Length == 0) return; // No streams connected

        int infectedNeighbours = 0;

        foreach (GameObject stream in streams){

            DataStreamObjectController streamScript = stream.GetComponent<DataStreamObjectController>();

            if (streamScript.returnInfectionStatus()){

                infectedNeighbours++;
                
                 if (coroutine == null)
                    {
                        coroutine = StartCoroutine(DamageProcess());
                    }

                isanyStreamInfected = true; // Set flag to true if any stream is infected
            }

            infectedMultiplier = infectedNeighbours;


        }

        if (!isanyStreamInfected && coroutine != null){

            infectedMultiplier = 1;
            StopCoroutine(coroutine);
            coroutine = null;
        }

    }

       

    IEnumerator DamageProcess()
    {
        

        while (currentHealth > 0f && renderer.material.color != endColor)
        {
            float healthPercentage = 1 - (currentHealth / maxHealth);
            renderer.material.color = Color.Lerp(startColor, endColor, healthPercentage);
             // Decrease health over time
             currentHealth = currentHealth - (damageSpeed * infectedMultiplier * Time.deltaTime);
            yield return null;
        }

        infected = true;
        isInfecting = false; // Infection is complete, flag set to false
        
    }


}