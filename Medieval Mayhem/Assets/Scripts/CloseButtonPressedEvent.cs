using UnityEngine;

public class CloseButtonPressedEvent : ButtonPressedEvent
{
	private const string AnimState = "State";	
	private const int AnimStateDisappear = 2;

	[SerializeField] private Animator _animTutorial;
	
	protected override void ButtonAction()
	{
		_animTutorial.SetInteger(AnimState, AnimStateDisappear);
	}
}
