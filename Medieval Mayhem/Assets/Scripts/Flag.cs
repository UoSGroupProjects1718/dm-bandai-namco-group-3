using UnityEngine;

public class Flag : MonoBehaviour
{
    private const string AnimState = "State";
    
    private const int AnimStateHidden = 0;
    private const int AnimStateRising = 1;
    private const int AnimStateFlag = 2;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Show()
    {
        _animator.SetInteger(AnimState, AnimStateFlag);
    }

    public void Rise()
    {
        _animator.SetInteger(AnimState, AnimStateRising);
    }

    public void Hide()
    {
        _animator.SetInteger(AnimState, AnimStateHidden);
    }
}
