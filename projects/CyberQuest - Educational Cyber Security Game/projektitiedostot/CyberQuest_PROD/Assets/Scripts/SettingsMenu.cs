using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

  void Start()
  {
    if (audioMixer.GetFloat("volume", out float currentVolume))
    {
      volumeSlider.value = currentVolume;
    }
  }

  public void SetVolume (float volume){
      Debug.Log(volume);
      audioMixer.SetFloat("volume",volume);
    }
}
