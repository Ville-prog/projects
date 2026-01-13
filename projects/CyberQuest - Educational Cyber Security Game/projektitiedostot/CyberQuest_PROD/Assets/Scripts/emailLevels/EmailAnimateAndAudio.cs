using UnityEngine;
using System.Collections;

public class EmailAnimateAndAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private GameObject emailIcon;
    private GameObject emailWindowScreen;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        emailIcon = GameObject.Find("Email_Icon");
        emailWindowScreen = GameObject.Find("emailAppWindow");  
    }


    public void PlayAudio()
    {
        audioSource.Play();
    }
    public IEnumerator animateEmailWindow(float duration)
    {
        Vector3 originalScale = emailWindowScreen.transform.localScale;
        Vector3 targetScale = originalScale * 1.1f;
        float elapsedTime = 0f;
        float speed = 5f;
    
        while (elapsedTime < duration)
        {
            float t = Mathf.PingPong(elapsedTime * speed, 1);
            emailWindowScreen.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        emailWindowScreen.transform.localScale = originalScale; // Reset scale
    }

    public IEnumerator animateEmailIcon(float duration)
    {
        float elapsedTime = 0f;
        float angle = 15f; 
        float speed = 5f; 

        while (elapsedTime < duration)
        {
            float t = Mathf.PingPong(elapsedTime * speed, 1); 
            float rotationZ = Mathf.Lerp(-angle, angle, t);
            emailIcon.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        emailIcon.transform.rotation = Quaternion.identity; // Reset rotation
    }
}
