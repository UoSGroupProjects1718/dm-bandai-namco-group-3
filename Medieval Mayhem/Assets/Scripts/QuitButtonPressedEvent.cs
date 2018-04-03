using UnityEngine;

public class QuitButtonPressedEvent : ButtonPressedEvent {

	protected override void ButtonAction()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	
}