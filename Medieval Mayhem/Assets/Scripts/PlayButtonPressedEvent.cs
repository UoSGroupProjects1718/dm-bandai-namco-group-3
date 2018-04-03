using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonPressedEvent : ButtonPressedEvent {

	protected override void ButtonAction()
	{
		new ScoreBoard().ResetScores();
		StartCoroutine(TransitionToLoadingScene());
	}

	private static IEnumerator TransitionToLoadingScene()
	{
		var transitionManager = Camera.main.GetComponent<TransitionManager>();
		transitionManager.PrepareSceneTransition(false);
		while (!transitionManager.ReadyForTransition)
			yield return null;
		SceneManager.LoadScene("Loading");
	}
	
}
