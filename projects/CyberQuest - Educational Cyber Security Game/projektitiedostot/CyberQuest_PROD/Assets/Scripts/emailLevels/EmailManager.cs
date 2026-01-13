using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class EmailManager : MonoBehaviour
{
    private hpManagerScript hpManager;

    private List<Email> emails = new List<Email>();
    public Sprite[] icons;
    private TextMeshProUGUI senderName;
    private TextMeshProUGUI senderEmail;
    public Image emailIcon;

    public Button acceptButton;
    public Button deleteButton;

    public int score = 0;
    public GameObject Canvas1;
    public GameObject Canvas2; 
    

    int emailIndex = 0;

    private EmailAnimateAndAudio emailAnimateAndAudio;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        hpManager = FindFirstObjectByType<hpManagerScript>();
        hpManager.hp = 3;
        hpManager.UpdateMask();

        Canvas1.SetActive(true);
        Canvas2.SetActive(false);
                
        //assign the correct gameobjects to the text components
        senderName = GameObject.Find("Email_Sender").GetComponent<TextMeshProUGUI>();
        senderEmail = GameObject.Find("Email_Message").GetComponent<TextMeshProUGUI>();
        emailIcon = GameObject.Find("Sender_Avatar").GetComponent<Image>();
        
        // Animaatiot ja äänet
        GameObject emailAnimationAndAudioObject = GameObject.Find("EmailAnimationAndAudio");
        emailAnimateAndAudio = emailAnimationAndAudioObject.GetComponent<EmailAnimateAndAudio>();
        emailAnimateAndAudio.PlayAudio();
        StartCoroutine(emailAnimateAndAudio.animateEmailWindow(0.35f));
        StartCoroutine(emailAnimateAndAudio.animateEmailIcon(0.627f));
       
        // create the instances of different emails

        Email bossEmail = new Email("Boss", @"
        Hey, I need you to come to my office later to 
        talk about the issue of fishing emails in our company. 
        Also, would you mind checking the following emails for me?
        Report them if they seem unsafe!", "safe", false);

        // The level always starts with this email
        emails.Add(bossEmail);

        emails.Add(new Email("Oskari", @"
        Hello, I am most definitely the main IT-security guy from
        our company, I would need you username and password to
        check that your safe", "dangerous", false));
       
        emails.Add(new Email("Lauri", @"
        I think forgot my keys to the
        company server room again, could you tell me
        the passcode to the door?
        
        I would be very thankful!", "dangerous", false));

        emails.Add(new Email("SpotifySupportServices", @"
        Hi! Your Spotify Premium is about to expire!
        To keep your music ad-free, 
        please update your payment details now.
       
        If you don’t renew within 24 hours, your account will revert
        to free status.", "dangerous", false));

        emails.Add(new Email("Grandma", @"
        Hey this is your grandmother here. I have forgotten my bank 
        password and can’t pay my rent. Could you please send
        me quickly some money to this bank address:

        xxxx-this-is-not-a-scam(Costa Rica)", "dangerous", false));

        emails.Add(new Email("HR Department", @"
        Hello Team, 

        We are updating our employee records and need you
        to confirm your personal details. Please fill out
        the attached form with your full name, address, 
        and banking information.  
        Failure to comply may result in payroll delays.  

        Best,  
        HR Team", "dangerous", false));
		
        emails.Add(new Email("John from IT", @"
        Hey,  

        There was an attempted security breach earlier today,
        and we need all employees to verify their credentials
        immediately. Please log in to our security portal
        using the link below to ensure your account remains 
        active: 

        Thanks,  
        IT Department", "dangerous", false));
    
        emails.Add(new Email("SpaceMarket Rewards", @"
        Congratulations! You have won a $500 SpaceMarket gift card!  
        To claim your prize, simply enter your email and password  
        on the official rewards page:  

        [Claim Your Gift]  

        Hurry! Offer expires in 24 hours.", "dangerous", false));

        emails.Add(new Email("CEO - Urgent Request", @"
        Hello,  
        I’m currently in a meeting and need you to purchase  
        $2,000 worth of gift cards for a client.  
        Please buy them ASAP and send me the codes here. 

        This is urgent, do not delay.  
        Thanks,  

        CEO", "dangerous", false));

        emails.Add(new Email("Team Meeting Reminder", @"
        Hi Team,  
        Just a quick reminder about our meeting tomorrow at
        10 AM in Conference Room B. Please be on time, and
        review the agenda
        attached.  

        Let me know if you have any questions!  

        Regards,  
        Team Leader", "safe", false));

        emails.Add(new Email("Team Lead", @"
        Hey everyone,  
        Just a quick update—our next project deadline has
        been pushed back by a week, so you have a little
        extra time to finalize your tasks. Let me know if
        you need any help.  

        Cheers,  
        Team Leader", "safe", false));

        emails.Add(new Email("Iron Gym", @"
        Hi Jamppa,  
        Just a reminder that your free trial at Iron Gym
        is ending soon! No worries if you’re not interested,
        but if you’d like to continue, we’d love to have you.
        Let us know if you have any questions!  

        Stay active,  
        Iron Gym Team", "safe", false));

        emails.Add(new Email("Colleague", @"
        Hey Jamppa,  
        I just wanted to follow up on that report we were working on.  
        Do you have time to review it together later today?  

        Let me know what works for you.  

        Best,  
        Your Colleague", "safe", false));

        // Shuffle the remaining emails and take 4 random ones
        List<Email> randomEmails = emails.Skip(1).OrderBy(x => UnityEngine.Random.value).Take(4).ToList();

        emails = new List<Email> { bossEmail };
        emails.AddRange(randomEmails);

        senderName.SetText(emails[0].Sender);
        senderEmail.SetText(emails[0].Body);
        emailIcon.sprite = icons[emailIndex];

        // buttons trigger the functions when clicked

        acceptButton.onClick.AddListener(() => OnButtonPress("accept"));
        deleteButton.onClick.AddListener(() => OnButtonPress("delete"));

    }

    void OnButtonPress(string buttonPressed){

        Email currentEmail = emails[emailIndex];


        if ((buttonPressed == "accept" && currentEmail.Threat == "safe") ||
         (buttonPressed == "delete" && currentEmail.Threat == "dangerous")){

                score++;
                currentEmail.AnsweredCorrectly = true;

         }

         else{

            currentEmail.AnsweredCorrectly = false;
            hpManager.reduceHp();
         }

        NextEmail();  
    }
    void NextEmail(){

        if (emailIndex < emails.Count-1){

            emailIndex++;

            senderName.SetText(emails[emailIndex].Sender);
            senderEmail.SetText(emails[emailIndex].Body);
            emailIcon.sprite = icons[emailIndex];

            // Animations and sounds
            emailAnimateAndAudio.PlayAudio();
            StartCoroutine(emailAnimateAndAudio.animateEmailWindow(0.35f));
            StartCoroutine(emailAnimateAndAudio.animateEmailIcon(0.627f));
            
        }
        else
        {
            if (hpManager.hp > 0) {
                SceneManager.LoadScene("Email_Level_pt2");
            }
        }
    }

    public int getScore(){

        return score;
    }

    public List<Email> GetEmails(){

        return emails;
    }
}

public class Email {

    private string sender;
    private string body;

    private string threat;

    private bool answeredCorrectly;

    public string Sender{

        get {return sender;}

    }

    public string Body{

        get {return body;}
    }

    public string Threat{

        get{return threat;}
    }

    public bool AnsweredCorrectly{
        get {return answeredCorrectly;}
        set {answeredCorrectly = value;}
    }

    public Email(string sender, string body, string threat, bool answeredCorrectly){


        this.sender = sender;
        this.body = body;
        this.threat = threat;
        this.answeredCorrectly = answeredCorrectly;

    }
}
