using UnityEngine;

public class HighlightController : MonoBehaviour
{
    public GameObject stepHighlights;


    public void ShowHighlights()
    {
        stepHighlights.SetActive(true);
    }

}
