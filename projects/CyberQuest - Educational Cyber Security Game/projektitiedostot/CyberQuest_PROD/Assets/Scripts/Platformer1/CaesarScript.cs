using System;
using TMPro;
using UnityEngine;


public class CaesarScript : MonoBehaviour
{   

    public int puzzleNum;
    
    private string[] correctwords = {"SALASANA", "SESAM", "CYBERSECURITY", "CAESAR"};
    private string correctword;
    private string encrypted;
    private int shift = 3 ;

    public TextAnim animation1;

    public TextMeshProUGUI alphabetText;
    public AlphabetAnimator alphabetAnimation;

    public TextMeshProUGUI wallText;
    private bool solved = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
       System.Random rd = new System.Random();

        int random_Number = rd.Next(1, 15);
        shift = random_Number;

        correctword = correctwords[puzzleNum];
        encrypted = Encrypt(correctwords[puzzleNum], shift);
        animation1.SetMessage(encrypted);
        String baseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        alphabetText.SetText(baseAlphabet.Substring(shift) + baseAlphabet.Substring(0, shift));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    String Encrypt(string word, int shift){

        char[] buffer = word.ToCharArray();



        for (int i = 0; i < buffer.Length; i++){


        int letter = buffer[i];
        char baseChar = 'A';

        letter = baseChar + (letter - baseChar + shift + 26) %26;

        buffer[i] = (char) letter;

        }

        return new string(buffer);

        
    }

    public void ShiftAlphabet(){

        string baseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string shiftedAlphabet = baseAlphabet.Substring(shift) + baseAlphabet.Substring(0, shift);

        alphabetText.SetText(shiftedAlphabet);
        alphabetAnimation.StartFontSizeAnimation();

    }

    public void ShiftLeft(){

        shift = (shift +1 + 26) %26;
        encrypted = Encrypt(encrypted, 1);
        animation1.SetMessage(encrypted);
        ShiftAlphabet();

        checkCorrect();
        
    }

    public void ShiftRight(){

        shift = (shift -1 + 26) %26;
        encrypted = Encrypt(encrypted, -1);
        animation1.SetMessage(encrypted);
        ShiftAlphabet();

        checkCorrect();

        
        }
    

    string getWord(){

        return encrypted;
    }

    public bool getSolvedorNot(){

        return solved;

    }

    void checkCorrect(){

        if (encrypted == correctword){
            

            Debug.Log("Correct Answer! Continue Game State!");
            solved = true;
            wallText.color = Color.cyan;
            
        }
    }
}
