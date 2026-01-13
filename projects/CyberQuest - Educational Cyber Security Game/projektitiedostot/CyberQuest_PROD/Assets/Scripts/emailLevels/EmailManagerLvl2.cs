using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailManagerLvl2 : MonoBehaviour
{
    private List<Dictionary<string, Email>> emailChains = new List<Dictionary<string, Email>>();
    private int currentEmailChainIndex = 0;
    private EmailAnimateAndAudio emailAnimateAndAudio;    private TextMeshProUGUI senderName;
    private TextMeshProUGUI senderEmail;
    private Button replyButton;
    private Button nextEmailButton;
    private Image replySpeechBubble;
    private Button[] replyMessageButtons;
    
    [SerializeField] private Sprite[] icons;
    public Image emailAvatar;

    private Email currentEmail;

    void Start()
    {
        emailAvatar = GameObject.Find("Sender_Avatar").GetComponent<Image>();
        
        senderName = GameObject.Find("Email_Sender").GetComponent<TextMeshProUGUI>();
        senderEmail = GameObject.Find("Email_Message").GetComponent<TextMeshProUGUI>();
        nextEmailButton = GameObject.Find("nextEmailButton").GetComponent<Button>();
       
        nextEmailButton.gameObject.SetActive(false); 

        GameObject emailAnimationAndAudioObject = GameObject.Find("EmailAnimationAndAudio");
        emailAnimateAndAudio = emailAnimationAndAudioObject.GetComponent<EmailAnimateAndAudio>();

        replySpeechBubble = GameObject.Find("ReplySpeechBubble").GetComponent<Image>();
        replySpeechBubble.transform.localScale = Vector3.zero;

        replyButton = GameObject.Find("replyButton").GetComponent<Button>();

        GameObject[] buttonObjects = GameObject.FindGameObjectsWithTag("replyMessageButton")
            .OrderBy(go => go.name).ToArray();
        replyMessageButtons = buttonObjects.Select(go => go.GetComponent<Button>()).ToArray();

        // Initialize email chains
        InitializeEmailChain1();
        InitializeEmailChain2();
        InitializeEmailChain3();
        InitializeEmailChain4();
        InitializeEmailChain5();

        // Shuffle and select 3 random email chains
        emailChains = emailChains.OrderBy(x => Random.value).Take(3).ToList();
    
        // Start with the first email in the shuffled list
        currentEmail = emailChains[0]["initial"];
        DisplayEmail(currentEmail);
    }

    void DisplayEmail(Email email)
    {
        senderName.SetText(email.Sender);
        senderEmail.SetText(email.Body);
        emailAvatar.sprite = icons[currentEmailChainIndex];

        for (int i = 0; i < replyMessageButtons.Length; i++)
        {
            if (i < email.Responses.Count)
            {
                replyMessageButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(email.Responses.Keys.ElementAt(i));
                int index = i;

                // Extract the nextEmail from the tuple
                var nextEmail = email.Responses.Values.ElementAt(i).nextEmail;

                replyMessageButtons[i].onClick.RemoveAllListeners();
                replyMessageButtons[i].onClick.AddListener(() => ReplyMessageChosen(nextEmail));
            }
        }
        emailAnimateAndAudio.PlayAudio();
        StartCoroutine(emailAnimateAndAudio.animateEmailWindow(0.35f));
        StartCoroutine(emailAnimateAndAudio.animateEmailIcon(0.627f));
        toggleReplyMessageButtonActive();
    }

    public void nextEmailButtonClicked()
    {
        // Check if this is the last email chain
        if (currentEmailChainIndex >= emailChains.Count - 1)
        {
            LevelLoader.LoadSceneStatic("emailVictory"); 
            return;
        }
        
        currentEmailChainIndex++;

        currentEmail = emailChains[currentEmailChainIndex]["initial"];
        DisplayEmail(currentEmail);

        toggleReplyMessageButtonActive();
        Debug.Log("next emailchain");
        replyButton.gameObject.SetActive(true);
        nextEmailButton.gameObject.SetActive(false);
    }

    public void ReplyMessageChosen(Email nextEmail)
    {
        // Find the selected response
        var selectedResponse = currentEmail.Responses.FirstOrDefault(r => r.Value.nextEmail == nextEmail);
    
        // Check if the response is a wrong answer
        if (selectedResponse.Value.isWrongAnswer)
        {
            hpManagerScript.instance.reduceHp(); // Call reduceHp if it's a wrong answer
        }
    
        // Proceed to the next email
        currentEmail = nextEmail;
        DisplayEmail(currentEmail);
        StartCoroutine(ScaleOverTime(replySpeechBubble.gameObject, 0.1f));
    
        if (currentEmail.IsEndOfDialogue)
        {
            nextEmailButton.gameObject.SetActive(true);
            replyButton.gameObject.SetActive(false);
        }
    }

    public void ClickReply()
    {
        replyButton.interactable = false;
        toggleReplyMessageButtonActive();
        StartCoroutine(ScaleOverTime(replySpeechBubble.gameObject, 0.1f));
    }

    public void toggleReplyMessageButtonActive()
    {
        bool isActive = !replyMessageButtons[0].gameObject.activeSelf;
    
        for (int i = 0; i < replyMessageButtons.Length; i++)
        {
            if (i < currentEmail.Responses.Count)
            {
                // Only toggle buttons that are relevant to the current email
                replyMessageButtons[i].gameObject.SetActive(isActive);
            }
            else
            {
                // Ensure unused buttons remain hidden
                replyMessageButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator ScaleOverTime(GameObject target, float duration)
    {
        Vector3 originalScale = target.transform.localScale;
        Vector3 targetScale = originalScale == Vector3.zero ? new Vector3(-1, 1, 1) : Vector3.zero;
        float currentTime = 0.0f;

        while (currentTime <= duration)
        {
            target.transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        target.transform.localScale = targetScale;
        
        if (!currentEmail.IsEndOfDialogue)
        {
            replyButton.interactable = true;
        }
    }

    public class Email
    {
        public string Sender { get; private set; }
        public string Body { get; private set; }
        public Dictionary<string, (Email nextEmail, bool isWrongAnswer)> Responses { get; private set; }
        public bool IsEndOfDialogue { get; private set; }

        public Email(string sender, string body, bool isEndOfDialogue = false)
        {
            Sender = sender;
            Body = body;
            Responses = new Dictionary<string, (Email, bool)>();
            IsEndOfDialogue = isEndOfDialogue;
        }

        public void AddResponse(string buttonText, Email nextEmail, bool isWrongAnswer = false)
        {
            Responses[buttonText] = (nextEmail, isWrongAnswer);
        }
    }

    void InitializeEmailChain1()
    {
        var emailChain1 = new Dictionary<string, Email>();

        // Initial email
        emailChain1["initial"] = new Email("Professor Smith", "Dear Recipient,\n\nI hope this email finds you well. I am Professor Smith from the Department of Advanced Cybersecurity at the University of Tornio. We are currently conducting groundbreaking research on quantum encryption, a technology that could revolutionize data security. However, we are in urgent need of additional funding to continue our work.\n\nYour contribution would be invaluable to our research and could help us achieve significant advancements in the field. Please consider transferring funds to the following account to support our efforts:\n\nAccount Number: 1234-5678-9012\nBank: Global Trust Bank\n\nThank you for your support.\n\nBest regards,\nProfessor Smith");

        // Responses to player
        emailChain1["learnMore"] = new Email("Professor Smith", "Dear Recipient,\n\nThank you for your interest in our research on quantum encryption! We are currently working on developing a new cryptographic algorithm that leverages quantum entanglement to ensure unbreakable security in data transmissions. Our findings so far have shown promising results, but we require additional resources to further validate and implement this technology in real-world applications.\n\nYour support could play a crucial role in pushing this groundbreaking research forward. If you have any further questions or would like to discuss this in more detail, please don't hesitate to reach out.\n\nBest regards,\nProfessor Smith");
        emailChain1["decline"] = new Email("Professor Smith", "Dear Recipient,\n\nI completely understand your decision. Funding research can be a significant commitment, and I appreciate your honesty. If you ever reconsider or know someone who might be interested in supporting our work, please don't hesitate to reach out. Your support, even in the future, would be greatly valued.\n\nThank you for your time, and I hope we can stay in touch.\n\nBest regards,\nProfessor Smith");
        emailChain1["scam"] = new Email("Professor Smith", "Dear Recipient,\n\nI assure you that this is a legitimate research effort conducted by the Department of Advanced Cybersecurity at the University of Tornio. We understand your concerns, especially given the prevalence of online scams, but we are more than willing to provide any information necessary to verify our work.\n\nScientific progress depends on trust and collaboration, and we are committed to maintaining transparency in all our endeavors.\n\nBest regards,\nProfessor Smith");

        // Respones 1
        emailChain1["sendFunds"] = new Email("Professor Smith", "Dear Recipient,\n\nThank you for your generous contribution! Your support will be instrumental in advancing our research on quantum encryption. We will keep you updated on our progress and ensure that your contribution is used effectively to achieve groundbreaking results.\n\nIf you have any further questions or would like to discuss our research in more detail, please don't hesitate to reach out.\n\nBest regards,\nProfessor Smith", true);
        emailChain1["refuseFunding"] = new Email("Professor Smith", "Dear Recipient,\n\nI understand your hesitation. Scientific breakthroughs often require trust and collaboration, and I respect your decision. If you ever change your mind or know someone who might be interested in supporting our work, please don't hesitate to reach out. Your support, even in the future, would be greatly valued.\n\nThank you for your time, and I hope we can stay in touch.\n\nBest regards,\nProfessor Smith", true);
        emailChain1["reportScam"] = new Email("Professor Smith", "Dear Recipient,\n\nI am deeply disappointed by your accusation. This research is a legitimate effort conducted by the Department of Advanced Cybersecurity at the University of Tornio. We have provided all necessary credentials and references to verify our work, and your accusation is both harmful and unfounded.\n\nIf you have any legitimate concerns, I encourage you to contact our department directly for clarification. Otherwise, I must insist that you refrain from making baseless accusations.\n\nBest regards,\nProfessor Smith", true);

        // Define additional emails for the "decline" and "scam" paths
        emailChain1["finalDecline"] = new Email("Professor Smith", "Dear Recipient,\n\nThank you for your response. I appreciate your honesty and understanding. If you ever reconsider or know someone who might be interested in supporting our work, please don't hesitate to reach out.\n\nBest regards,\nProfessor Smith", true);
        emailChain1["finalApology"] = new Email("Professor Smith", "Dear Recipient,\n\nThank you for your apology. I understand that misunderstandings can happen, especially in sensitive matters like funding requests. If you have any further questions or would like to verify our work, please don't hesitate to reach out.\n\nBest regards,\nProfessor Smith", true);
        emailChain1["scamProof"] = new Email("Professor Smith", "Dear Recipient,\n\nI understand your need for additional proof. Our department has a long history of groundbreaking research, and we are happy to provide any necessary documentation to verify our work.\n\nPlease feel free to contact us directly if you have any further questions.\n\nBest regards,\nProfessor Smith", true);

        // Define responses for the initial email
        emailChain1["initial"].AddResponse("1. I am interested in learning more.", emailChain1["learnMore"]);
        emailChain1["initial"].AddResponse("2. I cannot contribute at this time.", emailChain1["decline"]);
        emailChain1["initial"].AddResponse("3. This sounds like a scam.", emailChain1["scam"]);

        // Define responses for the "learnMore" email
        emailChain1["learnMore"].AddResponse("1. Ok, I'm sending the funds.", emailChain1["sendFunds"], true); // Wrong answer
        emailChain1["learnMore"].AddResponse("2. Sorry, I must refuse.", emailChain1["refuseFunding"], false); // Wrong answer
        emailChain1["learnMore"].AddResponse("3. No, this seems suspicious. I'm reporting you.", emailChain1["reportScam"], false); // Correct answer

        // Define responses for the "decline" email
        emailChain1["decline"].AddResponse("1. Thank you for understanding.", emailChain1["finalDecline"]);
        emailChain1["decline"].AddResponse("2. Can you provide more information?", emailChain1["learnMore"]);
        emailChain1["decline"].AddResponse("3. I still think this is a scam.", emailChain1["scam"]);

        // Define responses for the "scam" email
        emailChain1["scam"].AddResponse("1. I apologize for the misunderstanding.", emailChain1["finalApology"]);
        emailChain1["scam"].AddResponse("2. I need more proof.", emailChain1["scamProof"]);
        emailChain1["scam"].AddResponse("3. I'm reporting you.", emailChain1["reportScam"]);

        emailChains.Add(emailChain1);
    } 
    void InitializeEmailChain2()
    {
        var emailChain2 = new Dictionary<string, Email>();

        // Initial phishing email
        emailChain2["initial"] = new Email("Software Licensing Team", 
            "Dear User,\n\nOur records indicate that your company's software license will expire <b>tomorrow</b>. Immediate action is required to prevent <b>service disruption</b> and possible <b>data loss</b>. If payment is not received, access to all software functionalities will be revoked, potentially causing workflow interruptions and compliance issues.\n\nTo renew your license, please <b>submit your payment immediately</b> via the secure link below. Processing is fast and ensures your continued access without delay.\n\nFailure to act in time may result in costly reactivation fees. If you have already paid, you may disregard this message.\n\nBest regards,\nSoftware Licensing Team", false);

        // Player responses to the initial email
        emailChain2["clickLink"] = new Email("Software Licensing Team", 
            "Dear User,\n\nThank you for your renewal. We have successfully processed your payment, and your software license is now extended for another year. Your prompt action has ensured that there will be no disruption in your services.\n\nIf you require any further assistance, please contact our support team.\n\nBest regards,\nSoftware Licensing Team", true);

        emailChain2["askIT"] = new Email("IT Department", 
            "Hi,\n\nWe have reviewed the email you received and checked with our official vendor. <b>There is no pending renewal</b> for any of our company’s software licenses. This email appears to be <b>suspicious</b> and is likely a phishing attempt.\n\nDo <b>not</b> click on any links or enter payment information. We recommend that you forward this message to IT Security so that we can investigate further.\n\nThanks for verifying with IT first—great job staying cautious.\n\nStay safe,\nIT Security Team", false);

        emailChain2["ignore"] = new Email("Software Licensing Team", 
            "Dear User,\n\nThis is your final notice. Our records indicate that your software license has still not been renewed. If no payment is received within the next <b>24 hours</b>, your access will be <b>permanently revoked</b>, and you may face system-wide restrictions.\n\nAct now to avoid unnecessary disruptions.\n\nBest,\nSoftware Licensing Team", false);

        // Immediate scam report after ignoring
        emailChain2["reportScam"] = new Email("IT Security Team", 
            "Great job identifying this scam!\n\nWe've flagged the sender and reported them to the cybersecurity team for further action. <b>Always verify unexpected requests</b> before taking any action, especially those involving financial transactions.\n\nThanks for your diligence—your actions help keep the company secure.\n\nStay alert,\nIT Security Team", true);

        emailChain2["confrontScammer"] = new Email("Software Licensing Team", 
            "Dear User,\n\nThis is an <b>official</b> request, and your delay is putting your company at risk. We have been a trusted provider for years, and your software license is on the verge of expiring. If you fail to renew today, you will lose access to <b>critical services</b>, which could impact your operations significantly.\n\nDo not delay—ensure your renewal is processed immediately to avoid any complications.\n\nRegards,\nSoftware Licensing Team", false);

        // Player's final choice
        emailChain2["fallForScam"] = new Email("Software Licensing Team", 
            "Dear User,\n\nThank you for your payment. Your software license has been renewed successfully, and no further action is needed on your part.\n\n(Meanwhile, the scammers have stolen your payment information, and your funds have disappeared. The fraudsters have moved on to their next target...)\n\nBest,\nSoftware Licensing Team", true);

        emailChain2["blockScammer"] = new Email("IT Security Team", 
            "Dear User,\n\nWe have successfully blocked further emails from this sender and escalated the issue to authorities. <b>Great job recognizing the scam before falling for it!</b> Your attention to detail and quick thinking prevented a security breach.\n\nIf you receive any similar suspicious emails in the future, always verify their legitimacy with IT before taking any action.\n\nStay safe,\nIT Security Team", true);

        // Define responses for the initial email
        emailChain2["initial"].AddResponse("1. Click the link to renew the license.", emailChain2["clickLink"], true);
        emailChain2["initial"].AddResponse("2. Contact IT to verify the request.", emailChain2["askIT"], false);
        emailChain2["initial"].AddResponse("3. Ignore the email.", emailChain2["ignore"], false);

        // Responses after ignoring the initial email
        emailChain2["ignore"].AddResponse("1. Respond to the scammer (demand proof).", emailChain2["confrontScammer"], false);
        emailChain2["ignore"].AddResponse("2. Report this as a scam.", emailChain2["reportScam"], false);

        // Responses after asking IT
        emailChain2["askIT"].AddResponse("1. Report this as a phishing scam.", emailChain2["reportScam"], false);
        emailChain2["askIT"].AddResponse("2. Reply to the scammer and demand proof.", emailChain2["confrontScammer"], false);

        // Responses after confronting scammer
        emailChain2["confrontScammer"].AddResponse("1. Believe them and submit payment.", emailChain2["fallForScam"], true);
        emailChain2["confrontScammer"].AddResponse("2. Block their emails and report them.", emailChain2["blockScammer"], false);

        emailChains.Add(emailChain2);
    }
    void InitializeEmailChain3()
    {
        var emailChain3 = new Dictionary<string, Email>();

        // Initial phishing email
        emailChain3["initial"] = new Email("Bank Security Team",
            "Dear Customer,\n\n<b>URGENT: Suspicious Activity Detected on Your Account</b>\n\nWe have detected <b>unusual login attempts</b> from an unrecognized device. As a security measure, we have temporarily <b>restricted access</b> to your account.\n\nTo restore full access, please <b>verify your identity</b> by clicking the link below. If you do not take action within <b>24 hours</b>, your account may be <b>permanently suspended</b>, and you may lose access to your funds.\n\nIf you have already completed verification, please disregard this message.\n\n<b>Bank Security Team</b>");

        // Clicking the phishing link (falling for the scam)
        emailChain3["verify"] = new Email("Bank Security Team",
            "Dear Customer,\n\n<b>Thank you for verifying your account.</b>\n\nYour access has been restored, and all suspicious activity has been resolved.\n\nTo enhance security, please <b>update your password</b> and enable <b>two-factor authentication (2FA)</b>.\n\nIf you experience any issues, contact our support team at [customer-support@bank.com].\n\n<b>Best regards,</b>\nBank Security Team", true);

        // Ignoring the email (leads to a follow-up)
        emailChain3["ignore"] = new Email("Bank Security Team",
            "Dear Customer,\n\n<b>FINAL NOTICE: Account Suspension in Progress</b>\n\nWe have repeatedly attempted to contact you regarding <b>suspicious activity</b> on your account. Due to your lack of response, your account has been <b>temporarily suspended</b> for security reasons.\n\nTo restore access, please <b>contact our security team immediately</b> at 1-800-SECURE or visit <b>www.banksecurity.com/support</b>.\n\nFailure to act within <b>12 hours</b> may result in <b>permanent account limitations</b>.\n\n<b>Bank Security Team</b>", false);

        // Clicking the link after initially ignoring the email (final scam result)
        emailChain3["ignoreClick"] = new Email("Bank Security Team",
            "Dear Customer,\n\n<b>Thank you for verifying your account.</b>\n\nYour access has been restored, and no further action is required.\n\nIf you did <b>not</b> authorize this change, please <b>contact our fraud prevention team</b> immediately at 1-800-SECURE.\n\n<b>Best regards,</b>\nBank Security Team", true);

        // Reporting the scam (correct choice)
        emailChain3["reportScam"] = new Email("Bank Cybersecurity Team",
            "Dear Customer,\n\n<b>Thank you for reporting this phishing attempt.</b>\n\nOur cybersecurity team has <b>confirmed this email was fraudulent</b> and has taken immediate action to block the sender. We are also working with law enforcement to investigate this scam.\n\nIf you receive suspicious emails, do <b>not click on links</b> or provide personal information. Instead, report them immediately to <b>security@yourbank.com</b>.\n\n<b>Stay safe and vigilant,</b>\nYour Bank’s Cybersecurity Team", true);

        // Initial email responses
        emailChain3["initial"].AddResponse("1. Click the link to verify your account.", emailChain3["verify"], true);
        emailChain3["initial"].AddResponse("2. Ignore the email.", emailChain3["ignore"], false);
        emailChain3["initial"].AddResponse("3. Report this as a phishing scam.", emailChain3["reportScam"], false);

        // Responses after ignoring the initial email
        emailChain3["ignore"].AddResponse("1. Click the link to verify after all.", emailChain3["ignoreClick"], true);
        emailChain3["ignore"].AddResponse("2. Report this as a phishing scam.", emailChain3["reportScam"], false);

        emailChains.Add(emailChain3);
    }
    void InitializeEmailChain4()
    {
        var emailChain4 = new Dictionary<string, Email>();

        // Initial phishing email from "CEO"
        emailChain4["initial"] = new Email("CEO Turner", "Hi Jamppa,\n\nI'm in a critical meeting and need you to process an urgent wire transfer for a confidential company acquisition. Please <b>send $50,000 to the following account immediately</b> and reply once completed:\n\n<b>Account Number: 3921-8793-00</b>\n<b>Bank: Global Enterprise Holdings</b>.\n\nThis is extremely time-sensitive and confidential. I trust you to handle this without delays.\n\nRegards, Michael Turner CEO, Jamppa Enterprises", false);

        // Player responses to the initial email
        emailChain4["transferFunds"] = new Email("CEO Turner", "Dear Jamppa,\n\nThank you for handling this request so swiftly. Ensuring smooth financial operations is crucial, and your prompt action helps maintain the stability of our transactions. I appreciate your diligence and willingness to act quickly when needed.\n\nI will personally verify once the funds have been received and will update you accordingly. If there are any issues or additional steps required, I will reach out immediately.\n\nYour dedication to the company does not go unnoticed, and I sincerely appreciate your efficiency in resolving this matter.\n\nBest regards,\nMichael Turner\nCEO", true);

        emailChain4["askForDetails"] = new Email("CEO Turner", "Dear [Recipient],\n\nI don’t have time for unnecessary back-and-forth questions. This needs to be handled <b>immediately</b>. \n\nJust <b>process the transfer</b> and confirm with me once it’s done. This is an urgent, <b>high-level business matter</b>, and I expect it to be resolved without delay.\n\nThis transaction is <b>strictly confidential</b>. <b>Do not discuss it with anyone else</b> in the company. The last thing we need is unnecessary interference slowing this down.\n\nTime is of the essence—make this your top priority.\n\n- Michael", false);

        emailChain4["contactFinance"] = new Email("Finance Department", "Hi,\n\nWe've reviewed your inquiry, and after checking our internal records, we can confirm that <b>there is no legitimate transfer request</b> from Michael Turner.\n\nThis situation closely matches <b>known CEO impersonation scams</b> where attackers attempt to pressure employees into wiring funds urgently. <b>Do not process this transfer under any circumstances.</b>\n\nYou did the right thing by verifying first—well done! If you receive any further pressure to proceed, please escalate the matter directly to IT Security or upper management.\n\nStay vigilant, and thanks for helping keep the company secure.\n\nBest,\nFinance Team", false);

        emailChain4["ignoreEmail"] = new Email("CEO Turner", "Hello,\n\nI still <b>haven’t received confirmation</b> that this transfer has been completed. This delay is <b>causing major disruptions</b> for the company, and I need this resolved immediately.\n\nI don’t understand the holdup—this is a simple request, and I <b>expected it to be handled promptly</b>. If this isn’t processed within the next hour, there will be serious consequences. \n\n<b>Do not make me escalate this further.</b> Process the wire transfer <b>now</b> and confirm once it’s done.\n\n- Michael", false);

        // Branching paths
        emailChain4["confrontScammer"] = new Email("CEO Turner", "What exactly are you implying?\n\n<b>This is a direct order from me.</b> I don’t appreciate hesitation when I’ve made myself clear. This is a critical business matter, and every second wasted is costing us.\n\nYou were specifically chosen for this task because I expected discretion and efficiency. <b>If you fail to process this transfer, any negative consequences will fall squarely on you.</b> This will not be forgotten.\n\nI need confirmation <b>immediately</b>. Just get it done.\n\n- Michael", false);

        emailChain4["verifyWithCEO"] = new Email("Turner (Real)", "Hi,\n\nI want to make this absolutely clear—I <b>did NOT send that email</b>. You are dealing with a phishing scam. This is a well-known tactic where criminals impersonate executives to pressure employees into wiring money.\n\n<b>Do not respond</b> to the sender. Forward this email to IT Security immediately so they can investigate and block any further attempts.\n\nI appreciate your vigilance in catching this before any harm was done. You just protected the company from a serious financial threat—well done.\n\n- Michael Turner, CEO", true);

        emailChain4["reportToIT"] = new Email("IT Security Team", "Great job identifying the scam!\n\nWe’ve flagged the sender and are actively tracking their activity. These types of scams rely on urgency and intimidation, so always be cautious.\n\n<b>Never process unexpected wire transfers</b> without direct verbal confirmation from the actual sender. Scammers often use spoofed emails that look convincing, but verifying in person or over the phone is the best defense.\n\nWe’ll ensure this attempt is fully investigated and reported to the necessary authorities.\n\nStay alert and stay safe!\n\nIT Security Team", true);

        emailChain4["threatenScammer"] = new Email("CEO Turner", "This is absolutely ridiculous.\n\nI don’t have time for this level of incompetence. <b>You will regret ignoring my orders.</b> This will be noted, and I will ensure senior management is made aware of your failure to comply.\n\nThis is your <b>final warning</b>. If the transfer is not completed within the next 30 minutes, I will take further action. <b>Send the money now.</b> No more delays.\n\n- Michael", false);

        emailChain4["scammerGivesUp"] = new Email("CEO Turner", "(After a few hours, the scammer stops replying. Their aggressive attempts to manipulate you have failed. You successfully resisted the attack and prevented a major financial loss. Well done!)", true);

        emailChain4["fallForScam"] = new Email("Fake CEO", "Thank you for processing the transfer.\n\nI knew I could trust you to act quickly and follow instructions without hesitation. That kind of efficiency is exactly what makes you an asset to this company.\n\n(Meanwhile, the scammers have disappeared, leaving no trace. The money is gone… and it’s too late to undo the transfer.)", true);

        // Define responses for the initial email
        emailChain4["initial"].AddResponse("1. Send the wire transfer.", emailChain4["transferFunds"], true);
        emailChain4["initial"].AddResponse("2. Ask for more details.", emailChain4["askForDetails"]);
        emailChain4["initial"].AddResponse("3. Contact the Finance Department to confirm.", emailChain4["contactFinance"]);

        // Responses after asking for details
        emailChain4["askForDetails"].AddResponse("1. Send the transfer anyway.", emailChain4["transferFunds"], true);
        emailChain4["askForDetails"].AddResponse("2. Confront the sender and demand proof.", emailChain4["confrontScammer"]);

        // Responses after ignoring the email
        emailChain4["ignoreEmail"].AddResponse("1. Send the transfer under pressure.", emailChain4["transferFunds"], true);
        emailChain4["ignoreEmail"].AddResponse("2. Contact the real CEO to verify.", emailChain4["verifyWithCEO"]);

        // Responses after confronting scammer
        emailChain4["confrontScammer"].AddResponse("1. Send the transfer to avoid trouble.", emailChain4["fallForScam"], true);
        emailChain4["confrontScammer"].AddResponse("2. Report to IT instead.", emailChain4["reportToIT"]);
        emailChain4["confrontScammer"].AddResponse("3. Threaten the scammer.", emailChain4["threatenScammer"]);

        // Responses after threatening scammer
        emailChain4["threatenScammer"].AddResponse("1. Ignore further emails.", emailChain4["scammerGivesUp"]);
        emailChain4["threatenScammer"].AddResponse("2. Give in and send the transfer.", emailChain4["fallForScam"], true);

        // Responses after contacting Finance
        emailChain4["contactFinance"].AddResponse("1. Report the email to IT.", emailChain4["reportToIT"]);
        emailChain4["contactFinance"].AddResponse("2. Verify with the real CEO.", emailChain4["verifyWithCEO"]);

        emailChains.Add(emailChain4);
    }

    void InitializeEmailChain5()
    {
        var emailChain5 = new Dictionary<string, Email>();

        // Initial phishing email
        emailChain5["initial"] = new Email("HR Department", "Dear Employee,\n\n<b>URGENT: Mandatory Policy Update</b>\n\nAs part of our ongoing efforts to enhance workplace security and compliance, all employees are required to review and acknowledge the updated company policies.\n\nPlease <b>click the link below</b> to read and confirm your agreement. Failure to do so by <b>end of the day</b> may result in administrative action.\n\nBest,\n<b>HR Department</b>");

        // Response: Clicking the phishing link (falling for the scam)
        emailChain5["verify"] = new Email("HR Department", "Dear Employee,\n\n<b>Thank you for reviewing the updated policies.</b>\n\nYour compliance has been recorded. If you have any questions, please reach out to your HR representative.\n\nBest,\n<b>HR Department</b>", true);

        // Response: Ignoring the email (HR "follows up")
        emailChain5["ignore"] = new Email("HR Department", "Dear Employee,\n\n<b>Reminder: Policy Update Required</b>\n\nWe noticed you have not yet reviewed the updated company policies. Compliance is mandatory, and failure to acknowledge by <b>tomorrow</b> may result in restricted system access.\n\n[<b>Review Policy Update</b>]\n\nBest,\n<b>HR Department</b>");

        // Response: Reporting the phishing email
        emailChain5["reportScam"] = new Email("IT Security Team", "Dear Employee,\n\n<b>Thank you for reporting this phishing attempt.</b>\n\nOur security team has confirmed that this email was fraudulent. We have blocked the sender and are investigating further. Always verify unexpected requests before taking action.\n\nStay vigilant,\n<b>IT Security Team</b>", true);

        // If the player ignores the follow-up
        emailChain5["scammerFollowUp"] = new Email("HR Department", "Dear Employee,\n\n<b>Final Notice: Policy Compliance Required</b>\n\nThis is your <b>last reminder</b> to complete the mandatory policy review. Non-compliance may result in <b>account suspension</b>.\n\n[<b>Review Policy Update</b>]\n\nBest,\n<b>HR Department</b>");

        // Define responses for the initial email
        emailChain5["initial"].AddResponse("1. Click the link to review the policies.", emailChain5["verify"], true);
        emailChain5["initial"].AddResponse("2. Ignore the email.", emailChain5["ignore"], false);
        emailChain5["initial"].AddResponse("3. Report this as a phishing scam.", emailChain5["reportScam"], false);

        // Responses after ignoring
        emailChain5["ignore"].AddResponse("1. Click the link now.", emailChain5["verify"], true);
        emailChain5["ignore"].AddResponse("2. Continue ignoring.", emailChain5["scammerFollowUp"], false);

        // Responses after scammer follows up
        emailChain5["scammerFollowUp"].AddResponse("1. Report is as as a scam", emailChain5["reportScam"], false);
        emailChain5["scammerFollowUp"].AddResponse("2. Click the link", emailChain5["verify"], true);

        emailChains.Add(emailChain5);
    }

}

