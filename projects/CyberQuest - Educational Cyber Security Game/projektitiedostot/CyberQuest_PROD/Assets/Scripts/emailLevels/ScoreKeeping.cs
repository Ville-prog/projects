using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeping : MonoBehaviour
{
    public Sprite[] icons;    
    public TextMeshProUGUI finalScoreText;
    public EmailManager emailManager;
    public TextMeshProUGUI senderName1;
    public Image emailIcon1;

    public TextMeshProUGUI senderName2;
    public Image emailIcon2;

    public TextMeshProUGUI senderName3;
    public Image emailIcon3;

    public TextMeshProUGUI senderName4;
    public Image emailIcon4;

    public TextMeshProUGUI senderName5;
    public Image emailIcon5;

    public TextMeshProUGUI senderName6;
    public Image emailIcon6;
    public TextMeshProUGUI senderName7;
    public Image emailIcon7;
    public TextMeshProUGUI senderName8;
    public Image emailIcon8;

    public List<Email> emails ;

    public List<Sprite> correctimages;

    public Image correctImage1;
    public Image correctImage2;
    public Image correctImage3;
    public Image correctImage4;
    public Image correctImage5;
    public Image correctImage6;
    public Image correctImage7;
    public Image correctImage8;

    public Button closeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (emailManager == null){

            emailManager = FindAnyObjectByType<EmailManager>();
        }

       emails = emailManager.GetEmails();

       closeButton.onClick.AddListener(closeWindow);
    }

    // Update is called once per frame
    void Update()
    {   

    }


    public void updateScore(){

        emails = emailManager.GetEmails();

       

        senderName1.SetText(emails[0].Sender);
        emailIcon1.sprite = icons[0];

        if (emails[0].AnsweredCorrectly == true){
            correctImage1.sprite = correctimages[0];
        }

        else{

            correctImage1.sprite = correctimages[1];
        }
        

        senderName2.SetText(emails[1].Sender);
        emailIcon2.sprite = icons[1];

         if (emails[1].AnsweredCorrectly == true){
            correctImage2.sprite = correctimages[0];
        }

        else{

            correctImage2.sprite = correctimages[1];
        }
        

        senderName3.SetText(emails[2].Sender);
        emailIcon3.sprite = icons[2];

         if (emails[2].AnsweredCorrectly == true){
            correctImage3.sprite = correctimages[0];
        }

        else{

            correctImage3.sprite = correctimages[1];
        }

        senderName4.SetText(emails[3].Sender);
        emailIcon4.sprite = icons[3];

        if (emails[3].AnsweredCorrectly == true){
            correctImage4.sprite = correctimages[0];
        }

        else{

            correctImage4.sprite = correctimages[1];
        }

        senderName5.SetText(emails[4].Sender);
        emailIcon5.sprite = icons[4];

        

        if (emails[4].AnsweredCorrectly == true){
            correctImage5.sprite = correctimages[0];
            
        }

        else{

            correctImage5.sprite = correctimages[1];
             
        }

        senderName6.SetText(emails[5].Sender);
        emailIcon6.sprite = icons[5];

        if (emails[5].AnsweredCorrectly == true){
            correctImage6.sprite = correctimages[0];
            
        }

        else{

            correctImage6.sprite = correctimages[1];
             
        }

        senderName7.SetText(emails[6].Sender);
        emailIcon7.sprite = icons[6];

        if (emails[6].AnsweredCorrectly == true){
            correctImage7.sprite = correctimages[0];
            
        }

        else{

            correctImage7.sprite = correctimages[1];
             
        }

        senderName8.SetText(emails[7].Sender);
        emailIcon8.sprite = icons[7];

        if (emails[7].AnsweredCorrectly == true){
            correctImage8.sprite = correctimages[0];
            
        }

        else{

            correctImage8.sprite = correctimages[1];
             
        }


        
 
 
 
       
        int final_score = emailManager.getScore();
        finalScoreText.SetText("Your final score is: " + final_score.ToString() + "!");
    }

    

    
    



    void closeWindow(){

         

            Application.Quit();
        

    }



}


public class Score{



}
