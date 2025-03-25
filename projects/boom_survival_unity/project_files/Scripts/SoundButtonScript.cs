using UnityEngine;
using UnityEngine.UI;

/*
 * This class is responsible for handling the sound toggle button UI component.
 * It updates the button image to reflect the current sound state (on/off) and
 * toggles the audio listener volume accordingly.
 */
public class SoundButtonScript : MonoBehaviour
{
    // "[SerializeField]" allows private fields to be visible and editable in
    // the Unity Inspector.
    [SerializeField] private Sprite SoundOnBtn;
    [SerializeField] private Sprite SoundOffBtn;
    private Image buttonImage;
    
    private bool isSoundOn = true;
    private float maxVolume;

    /*
     * Start is called before the first frame update
     */ 
    void Start()
    {
        maxVolume = AudioListener.volume;

        buttonImage = GetComponent<Image>();    
        buttonImage.sprite = SoundOnBtn; 
    }
    
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        
        buttonImage.sprite = isSoundOn ? SoundOnBtn : SoundOffBtn;
        AudioListener.volume = isSoundOn ? maxVolume : 0;
    }

}
