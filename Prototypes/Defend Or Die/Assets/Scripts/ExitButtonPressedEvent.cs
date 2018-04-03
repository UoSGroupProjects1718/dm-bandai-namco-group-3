using UnityEngine;

namespace Scenes.Subject_Selection.Scripts
{
	public class ExitButtonPressedEvent : ButtonPressedEvent {

		protected override void ButtonAction()
		{
			Debug.Log("HEY");
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
	
	}
}
