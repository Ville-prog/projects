using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggle : MonoBehaviour
{
  private bool isInitializing = true;
  void Start()
  {
    Toggle toggleComponent = GetComponent<Toggle>();
    if(toggleComponent != null){
      toggleComponent.isOn = Screen.fullScreen; // sets toggle to correct position
      
      isInitializing = false;
    }
  }
  public void FullScreen(){
    // doesn't trigger before toggle is set correct position
    if(isInitializing){
      return;
    }
    Debug.Log("fullscreen toggled");
    Screen.fullScreen = !Screen.fullScreen;
  }
}
