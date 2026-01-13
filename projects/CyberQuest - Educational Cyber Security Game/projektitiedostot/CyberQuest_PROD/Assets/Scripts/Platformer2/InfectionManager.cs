using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfectionManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] infectionPoints;

    [SerializeField]
    private float infectionDelay = 20f; // Delay between infections in seconds
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(InfectPointsCoroutine(infectionDelay)); // Start the infection coroutine
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InfectPointsCoroutine(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            InfectatPoints();
        }
    }

    void InfectatPoints(){

        GameObject pointtoInfect = infectionPoints[Random.Range(0, infectionPoints.Length)];
        ButtonHandler buttonHandler = pointtoInfect.GetComponent<ButtonHandler>();
        buttonHandler.OnButtonClick();
        Debug.Log("Infecting point: " + pointtoInfect.name);


    }
}
