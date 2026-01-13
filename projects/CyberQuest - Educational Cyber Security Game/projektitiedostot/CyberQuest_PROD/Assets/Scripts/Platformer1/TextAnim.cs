using System.Collections;
using TMPro;
using UnityEngine;



public class TextAnim : MonoBehaviour

{


[SerializeField]
private string _message;

[SerializeField]
private float _stringAnimationDuration;

[SerializeField]
private TextMeshProUGUI _animatedText;

[SerializeField]
[Range(0.0001f, 1)]
private float _charAnimationDuration;

[SerializeField]
AnimationCurve _sizeCurve;

[SerializeField]
private float _sizeScale;

[SerializeField]
[Range(0,1)]
float _editorTvalue;

private float _timeElapsed;
private Coroutine animationCoroutine;

void Start(){

    StartCoroutine(RunAnimation(1));
}

void Update(){

    EvaluateRichText(_editorTvalue);
}

public void SetMessage(string newMessage)
    {   
        Debug.Log(_stringAnimationDuration);
        _message = newMessage;
        _timeElapsed = 0;  // Reset the timer to restart the animation
        
        if (animationCoroutine != null)
    {
        StopCoroutine(animationCoroutine);
    }

    animationCoroutine = StartCoroutine(RunAnimation(0)); // Restart the animation with the new text
        
    }
IEnumerator RunAnimation(float waitForSeconds){

    yield return new WaitForSeconds(waitForSeconds);

    float t = 0;

    while (t <= 1f){

        EvaluateRichText(t);
        t = _timeElapsed / _stringAnimationDuration;
        _timeElapsed += Time.deltaTime;

        yield return null;
    }
}

void EvaluateRichText(float t){

    _animatedText.text = "";


    for (int i = 0; i < _message.Length; i++){

        _animatedText.text += EvaluateCharRichText(_message[i], _message.Length, i, t);

        
    }
   

    // Set the formatted text
    _animatedText.SetText(_animatedText.text);

    

}


private string EvaluateCharRichText(char c, int sLength, int cPosition, float t)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    {
        float startPoint = (1 - _charAnimationDuration) / (sLength - 1) * cPosition;
        float endPoint = startPoint + _charAnimationDuration;

         // Force the end point for the last character to be 1
        if (cPosition == sLength - 1) {
            endPoint = 1f;
        }
            
        float subT = t.Map(startPoint, endPoint, 0, 1);

        float sizeValue = _sizeCurve.Evaluate(subT) * _sizeScale;
    

        string sizeStart = $"<size={_sizeCurve.Evaluate(subT) * _sizeScale}%>";
        string sizeEnd = "</size>";

        return sizeStart + c + sizeEnd;
    }


}

public static class Extensions
{

    public static float Map(this float value, float fromLow, float fromHigh, float toLow, float toHigh) {
    return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
}
}
