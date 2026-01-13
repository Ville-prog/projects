using UnityEngine;
using System.Collections;
public class TimerManager : MonoBehaviour
{   

    public int duration;
    public int timer = 0;

    [SerializeField]
    private HealthBar healthBar;

    [SerializeField]
    private CanvasGroup levelPassedScreen;

    [SerializeField]
    private CanvasGroupFader fader;



    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        healthBar.SetMaxHealth(duration);
        startTimer();
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void startTimer(){

        StartCoroutine(runTimer());
    }

     IEnumerator runTimer(){

        

        while (timer < duration ){

            // Update the progress bar here
            // You can use a coroutine or a separate method to handle the timer logic
            // For example, you can use Time.deltaTime to decrease the duration
            timer += 1;
            healthBar.SetHealth(timer);
            yield return new WaitForSeconds(1f); // Example scaling based on duration
        }

        //Timer is fully complete

        StartCoroutine(fader.FadeIn(levelPassedScreen, 1f));
        Time.timeScale = 0f;

    }
}
