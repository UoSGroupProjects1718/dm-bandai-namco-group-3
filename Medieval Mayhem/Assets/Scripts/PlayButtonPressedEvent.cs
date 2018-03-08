using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonPressedEvent : ButtonPressedEvent {

	protected override void ButtonAction()
	{
		StartCoroutine(TransitionToLoadingScene());
	}

	private static IEnumerator TransitionToLoadingScene()
	{
		var transitionManager = Camera.main.GetComponent<TransitionManager>();
		transitionManager.PrepareTransition();
		while (!transitionManager.ReadyForTransition)
			yield return null;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
}
