using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
	[SerializeField] private Slider _slider;
    
	private AsyncOperation _asyncOperation;

	private void Start()
	{
		StartCoroutine(LoadGame());
	}
    
	private IEnumerator LoadGame()
	{
		yield return new WaitForSeconds(1.0f);
		_asyncOperation = SceneManager.LoadSceneAsync("Game");
		_asyncOperation.allowSceneActivation = false;

		while (_slider.value < 0.9f)
		{
			_slider.value = _asyncOperation.progress;
			if (_slider.value >= 0.9f)
			{
				_slider.value = 1.0f;
				
				var transitionManager = Camera.main.GetComponent<TransitionManager>();
				transitionManager.PrepareSceneTransition(true);
				while (!transitionManager.ReadyForTransition)
					yield return null;
				
				_asyncOperation.allowSceneActivation = true;
				break;
			}
			yield return null;
		}
	}
    
}
