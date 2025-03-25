using UnityEngine;
using UnityEngine.UI;

/*
 * This class is responsible for handling the slider UI component 
 * for choosing worldSize. It updates the displayed text to reflect the current
 * slider value and provides a method to retrieve the current slider value.
 */
public class SliderScript : MonoBehaviour
{
    // "[SerializeField]" allows private fields to be visible and editable in
    // the Unity Inspector.
    [SerializeField] private Slider slider;
    [SerializeField] private Text sliderText;
    
    private float sliderValue; 

    /*
     * Start is called before the first frame update
     */ 
    void Start()
    {
        UpdateSliderText(slider.value);

        slider.onValueChanged.AddListener(UpdateSliderText);
    }

    void UpdateSliderText(float value)
    {
        sliderValue = value; 
        sliderText.text = $"World size (width in blocks): {value}";
    }

    public float GetSliderValue()
    {
        return sliderValue;
    }
}
