using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

	[SerializeField] private AudioSource _audioSource;

	[SerializeField] private bool _fadeIn;
	[SerializeField] private float _fadeTime;

	private void Start()
	{
		if (_fadeIn)
		{
			StartMusic(_fadeTime);
		}
	}

	IEnumerator fadeMusic(float from, float to, float time)
	{
		float elapsedTime = 0;
		float currentVolume = from;

		if (time == 0.0f)
		{
			_audioSource.volume = to;
		}

		while(elapsedTime < time) {
			elapsedTime += Time.deltaTime;
			_audioSource.volume = Mathf.Lerp(currentVolume, to, elapsedTime / time);
			yield return null;
		}

		if (_audioSource.volume <= 0.0f)
		{
			_audioSource.Stop();
		}
	}

	public void StartMusic(float fadeTime)
	{
		_audioSource.Play();
		StartCoroutine(fadeMusic(0.0f, 1.0f, fadeTime));
	}

	public void StopMusic(float fadeTime)
	{
		StartCoroutine(fadeMusic(1.0f, 0.0f, fadeTime));
	}
	
}
