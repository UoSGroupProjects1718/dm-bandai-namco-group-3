using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	private const float VolumeMin = 0.0f;
	private const float VolumeMax = 0.4f;
	private const float FadeTimeDefault = 1.0f;

	public static AudioManager Instance
	{
		get { return _instance ?? (_instance = (AudioManager) FindObjectOfType(typeof(AudioManager))); }
	}
	
	private static AudioManager _instance;
	private AudioSource _audioSource;
	private bool _isFading;

	public void Play(AudioClip audioClip, float fadeInTime)
	{
		_audioSource.clip = audioClip;
		_audioSource.volume = VolumeMin;
		_audioSource.Play();
		StartCoroutine(FadeVolume(VolumeMax, fadeInTime));
	}
	
	public void Play(AudioClip audioClip)
	{	
		Play(audioClip, FadeTimeDefault);
	}

	public void Stop(float fadeOutTime)
	{
		StartCoroutine(FadeVolume(VolumeMin, fadeOutTime));
	}
	
	public void Stop()
	{
		Stop(FadeTimeDefault);
	}
	
	private IEnumerator FadeVolume(float desiredVolume, float fadeTime)
	{
		if (_isFading)
			Debug.LogWarningFormat("[{0}]: Attempted to fade music volume whilst a fade was already in progress.", GetType().Name);
		while (_isFading)
			yield return null;

		_isFading = true;

		if (desiredVolume == VolumeMin && fadeTime == 0.0f)
		{
			_audioSource.volume = 0.0f;
		}
		else if (desiredVolume < _audioSource.volume)
		{
			while (_audioSource.volume > desiredVolume)
			{
				_audioSource.volume -= Time.deltaTime / fadeTime;
				yield return null;
			}
		}
		else if (desiredVolume > _audioSource.volume)
		{
			while (_audioSource.volume < desiredVolume)
			{
				_audioSource.volume += Time.deltaTime / fadeTime;
				yield return null;
			}
		}

		if (_audioSource.volume <= VolumeMin)
		{
			_audioSource.Stop();
		}

		_isFading = false;
	}
	
	private void Awake()
	{
		if (Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_audioSource = GetComponent<AudioSource>();

			DontDestroyOnLoad(gameObject);
			
		}
	}

}
