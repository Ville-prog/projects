using UnityEngine;
using System.Collections;

public class DataStreamObjectController : MonoBehaviour
{
    [SerializeField]
    private bool infected;

    [SerializeField]
    private GameObject[] previous;

    [SerializeField]
    private Material mat;

    private Renderer renderer;

    private bool isInfecting = false; // Prevents multiple coroutines
    private bool isDeinfecting = false; // Prevents multiple coroutines for de-infection

    private Color startColor = new Color(0f, 1f, 0f), endColor = new Color(1f, 0f, 0f);

    private float infectedPeriod;
    private float cleansedPeriod = 30f;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        mat = renderer.material; // Get the material from the renderer
        
    }

    void Update()
    {   

       
        if (!infected){

            checkNeighbourInfection();

        }
        

        else if(infected){

            
            checkNeighbourCleanse();

            }
        }

    

    void checkNeighbourInfection(){

        if (previous.Length == 0) return; 

        foreach (GameObject neighbour in previous){

            DataStreamObjectController neighbourScript = neighbour.GetComponent<DataStreamObjectController>();
            
            if (neighbourScript.returnInfectionStatus()){
                startInfection();
                break; // Exit loop if any neighbour is infected
            }
        }
    }

    void checkNeighbourCleanse(){

        if (previous.Length == 0) return;
        bool allClean = true;

        foreach (GameObject neighbour in previous){

            DataStreamObjectController neighbourScript = neighbour.GetComponent<DataStreamObjectController>();
            
            if (neighbourScript.returnInfectionStatus()){
                allClean = false;
                break;
                
            }
        }

        if (allClean){
            startCleanse();
        }
    }

    /**
    * Function for starting infection by malicious actor
    */
    public void startInfection()
    {
        if (!infected && !isInfecting) // Only infect if not already infected and no infection is in progress
        {
            StartCoroutine(InfectionProcess());
        }
    }

    public void startCleanse(){
        
        if(infected && !isDeinfecting)

            StartCoroutine(cleanseProcess());
        }
        
    
    

    IEnumerator InfectionProcess()
    {
        isInfecting = true; // Mark infection process as running
        float tick = 0f;
        float speed = 0.25f;
        float width = transform.localScale.x; // Assuming uniform scale for simplicity

         // Set the width of the material
        while (tick < 1f)
        {
            tick += Time.deltaTime * speed;
            float infectionProgress = Mathf.Clamp01(tick);
            mat.SetFloat("_InfectionProgress", infectionProgress);
           
           
        
            yield return null;
        }

        mat.SetInt("_isInfected", 1);
        mat.SetFloat("_InfectionProgress", 0f); // Set the infection progress to 1 (fully infected)
        
       // Set the infected flag in the shader
        infected = true;
        isInfecting = false; // Infection is complete, flag set to false
        infectedPeriod = 0; // Reset infected period
    }

    IEnumerator cleanseProcess(){

        isInfecting = false; // Mark infection process as running
        float tick = 0f;
        float speed = 0.5f;

        while (tick < 1f)
        {
            tick += Time.deltaTime * speed;
            float infectionProgress = Mathf.Clamp01(tick);
            mat.SetFloat("_InfectionProgress", infectionProgress);

            // Set info to shader to adjust the animation
            
            yield return null;
        }

        // Set the infected flag to false in the shader

        mat.SetInt("_isInfected", 0);
        mat.SetFloat("_InfectionProgress", 0f);
        infected = false;
        isInfecting = false; // Infection is complete, flag set to false
        infectedPeriod = 0; // Reset infected period
        

    }

    

    public bool returnInfectionStatus(){
        return infected;
    }



} 

