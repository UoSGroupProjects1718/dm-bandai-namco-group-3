using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextTask : MonoBehaviour, ICoroutineTask
{
    private const string TextAnimState = "State";
        
    private const int TextAnimStateZoomIn = 1;
    private const int TextAnimStateZoomOut = 2;

    private Text _headingText;
    private Animator _headingTextAnimator;
    
    public bool IsComplete { get; set; }
    
    public void SetTextAndAnimator(Text headingText, Animator headingTextAnimator)
    {
        _headingText = headingText;
        _headingTextAnimator = headingTextAnimator;
    }

    public void SetText(string text)
    {
        _headingText.text = text;
    }

    public void SetColour(Color color)
    {
        _headingText.color = color;
    }

    private IEnumerator PerformAnimation(int desiredState)
    {
        IsComplete = false;
        _headingTextAnimator.SetInteger(TextAnimState, desiredState);
        yield return new WaitForSeconds(_headingTextAnimator.GetCurrentAnimatorClipInfo(0).Length);
        IsComplete = true;
    }

    public void ZoomIn()
    {
        StartCoroutine(PerformAnimation(TextAnimStateZoomIn));
    }
    
    public void ZoomOut()
    {
        StartCoroutine(PerformAnimation(TextAnimStateZoomOut));
    }
    
}