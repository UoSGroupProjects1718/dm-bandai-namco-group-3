using UnityEngine;

public class HelpButtonPressedEvent : ButtonPressedEvent
{
	private const string AnimState = "State";	
	private const int AnimStateAppear = 1;

	[SerializeField] private Animator _animTutorial;
	
	protected override void ButtonAction()
	{
		_animTutorial.SetInteger(AnimState, AnimStateAppear);
	}
}
