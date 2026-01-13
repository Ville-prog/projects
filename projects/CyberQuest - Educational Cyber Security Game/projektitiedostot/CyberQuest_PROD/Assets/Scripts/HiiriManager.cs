using UnityEngine;

public class HiiriManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D onClickTexture;
    [SerializeField] private AudioSource clickAudio;

    void Start()
    {
        // Set cursor
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);    
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // When clicked
        {
            Cursor.SetCursor(onClickTexture, Vector2.zero, CursorMode.Auto);
            clickAudio.Play(); // Play the audio source
        }
        else if (Input.GetMouseButtonUp(0)) // When released
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}
