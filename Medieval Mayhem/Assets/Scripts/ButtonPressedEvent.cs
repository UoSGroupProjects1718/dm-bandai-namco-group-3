using UnityEngine;

public abstract class ButtonPressedEvent : MonoBehaviour
{
    [SerializeField] private int _timesPressable;
    protected int TimesPressed;

    protected abstract void ButtonAction();

    public void PerformAction()
    {
        if (TimesPressed < _timesPressable)
        {
            ButtonAction();
            TimesPressed++;
        }
    }
}
