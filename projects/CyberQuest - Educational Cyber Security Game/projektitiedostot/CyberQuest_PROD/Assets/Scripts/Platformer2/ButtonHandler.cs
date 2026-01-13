using UnityEngine;
using UnityEngine.UI;
public class ButtonHandler : MonoBehaviour
{

    [SerializeField]
    private GameObject boxToInfect;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { // Add the OnButtonClick method to the button's click event
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        // Call the function to infect the box
        Debug.Log("Button clicked! Infecting box: " + boxToInfect.name);
        InfectBox();
    }

    public void OnCleanseButtonClick(){

        DataStreamObjectController dataStreamObjectController = boxToInfect.GetComponent<DataStreamObjectController>();
        Debug.Log("Button clicked! Cleansing box: " + boxToInfect.name);

        dataStreamObjectController.startCleanse();
    }

    private void InfectBox()
    {
        
        // Get the DataStreamObjectController component from the box
        Debug.Log("Infecting box: " + boxToInfect.name);
        DataStreamObjectController dataStreamObjectController = boxToInfect.GetComponent<DataStreamObjectController>();

        if (dataStreamObjectController != null)
        {
            // Call the method to infect the box
            dataStreamObjectController.startInfection();
        }
        else
        {
            Debug.LogError("DataStreamObjectController component not found on the specified box.");
        }
    }
}
