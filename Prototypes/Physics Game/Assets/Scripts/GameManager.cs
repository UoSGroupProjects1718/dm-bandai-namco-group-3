using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Exploder2D;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	private const float PlatformPositionMinX = -80.0f;
	private const float PlatformPositionMaxX = 80.0f;
	private const float PlatformPositionMinY = 0.0f;
	private const float PlatformPositionMaxY = 40.0f;

	private const int TextAnimStateIdle = 0;
	private const int TextAnimStateAppear = 1;
	private const int TextAnimStateDisappear = 2;
	private const int TextAnimStateTeamWins = 3;

	private const string TextAnimState = "State";

	private const string PlatformAnimState = "State";

	private const int PlatformAnimStateHidden = 0;
	private const int PlatformAnimStateFadeIn = 1;
	private const int PlatformAnimStateFadeOut = 2;


	[SerializeField] private GameObject _btnCheck;
	[SerializeField] private GameObject _objCannon;
	
	private bool _coroutineRunning;
	
	[SerializeField] private PlatformCollection _platformCollection;

	private List<GameObject> _redPlatforms = new List<GameObject>();
	private List<GameObject> _bluePlatforms = new List<GameObject>();

	[SerializeField] private GameObject _imageViewer;
	
	[SerializeField] private Sprite _sprRedTeamWin;
	[SerializeField] private Sprite _sprRedTeamArrange;
	[SerializeField] private Sprite _sprRedTeamTap;

	[SerializeField] private Sprite _sprBlueTeamWin;
	[SerializeField] private Sprite _sprBlueTeamArrange;
	[SerializeField] private Sprite _sprBlueTeamTap;



	private bool active;
	private bool potato;

	public static GameManager Instance
	{
		get { return _instance ?? (_instance = GameObject.FindObjectOfType<GameManager>()); }
	}
	
	private static GameManager _instance;
	
	private Team _activeTeam;

	private IEnumerator ShowImageForSeconds(Sprite sprite, float seconds)
	{
		_coroutineRunning = true;

		// If the animator is currently busy, wait for it to return to the idle state.
		var imageAnimator = _imageViewer.GetComponent<Animator>();		
		while (imageAnimator.GetInteger(TextAnimState) != TextAnimStateIdle)
			yield return null;		
		
		// Set the sprite of the image viewer and begin the appear animation.
		_imageViewer.GetComponent<SpriteRenderer>().sprite = sprite;
		_imageViewer.GetComponent<Animator>().SetInteger(TextAnimState, TextAnimStateAppear);
		
		// Wait for the appear animation to complete
		yield return new WaitForSeconds(_imageViewer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
		
		// Wait for the specified period.
		yield return new WaitForSeconds(seconds);
		
		// Start the disappear animation and wait for it to complete.
		_imageViewer.GetComponent<Animator>().SetInteger(TextAnimState, TextAnimStateDisappear);
		yield return new WaitForSeconds(_imageViewer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
		
		// Return the image viewer to its idle state.
		_imageViewer.GetComponent<Animator>().SetInteger(TextAnimState, TextAnimStateIdle);
		
		_coroutineRunning = false;
	}
	
	private IEnumerator ShowImageUntilTapped(Sprite sprite)
	{
		_coroutineRunning = true;
		
		// If the animator is currently busy, wait for it to return to the idle state.
		var imageAnimator = _imageViewer.GetComponent<Animator>();		
		while (imageAnimator.GetInteger(TextAnimState) != TextAnimStateIdle)
			yield return null;		
		
		// Set the sprite of the image viewer and begin the appear animation.
		_imageViewer.GetComponent<SpriteRenderer>().sprite = sprite;
		_imageViewer.GetComponent<Animator>().SetInteger(TextAnimState, TextAnimStateAppear);
		
		// Wait for the appear animation to complete
		yield return new WaitForSeconds(_imageViewer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
		
		// Wait until the screen is tapped.
		while (Input.touchCount == 0)
			yield return null;
		
		// Start the disappear animation and wait for it to complete.
		_imageViewer.GetComponent<Animator>().SetInteger(TextAnimState, TextAnimStateDisappear);
		yield return new WaitForSeconds(_imageViewer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
		
		// Return the image viewer to its idle state.
		_imageViewer.GetComponent<Animator>().SetInteger(TextAnimState, TextAnimStateIdle);

		_coroutineRunning = false;
	}
	
	
	private void ShowImage(Sprite image)
	{
		_imageViewer.GetComponent<SpriteRenderer>().sprite = image;
		_imageViewer.GetComponent<Animator>().SetInteger("State", 1);
	}


	private void Update()
	{
		Debug.LogFormat("Coroutine: {0}, Anim State: {1}", _coroutineRunning, _imageViewer.GetComponent<Animator>().GetInteger(TextAnimState));
		Debug.LogFormat("Touches: {0}", Input.touchCount);
		
		if (Input.GetKey(KeyCode.Space))
		{
			SceneManager.LoadScene(0, LoadSceneMode.Single);
		}
		
		if (!active) return;

		if (!potato && GameObject.FindGameObjectWithTag(Bomb.Tag))
		{
			potato = true;
		}

		if (potato)
		{
			Debug.Log("POTATO");
			if (!GameObject.FindGameObjectWithTag(Bomb.Tag))
			{
				_imageViewer.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Red Castle Explosion") != null
					? _sprBlueTeamWin
					: _sprRedTeamWin;
				
				for(int i = 0; i < 100; i++)
					Debug.Log("WIN!!!!");

				foreach (var platform in _redPlatforms)
					platform.GetComponent<Exploder2DObject>().Explode();
				foreach (var platform in _bluePlatforms)
					platform.GetComponent<Exploder2DObject>().Explode();
				
				_imageViewer.GetComponent<Animator>().SetInteger(TextAnimState, TextAnimStateTeamWins);
				active = false;
			}
		}
	}
	
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		StartCoroutine(SetupGame());
	}


	private Vector3 GetRandomPlatformPosition()
	{
		return new Vector3(Random.Range(PlatformPositionMinX, PlatformPositionMaxX),
			Random.Range(PlatformPositionMinY, PlatformPositionMaxY));
	}

	private void ResetPlatforms()
	{
		// Ensure that are any existing platforms are removed.
		
		if (_redPlatforms.Any())
		{
			foreach(var platform in _redPlatforms)
				Destroy(platform);
			_redPlatforms.Clear();
		}
		
		if (_bluePlatforms.Any())
		{
			foreach(var platform in _bluePlatforms)
				Destroy(platform);
			_bluePlatforms.Clear();
		}
		
		// Obtain a random set of platforms and randomly place them
		
		var platforms = _platformCollection.GetRandomPlatforms(4);
		foreach (var set in platforms)
		{
			var redPlatform = set.Item1;
			redPlatform.transform.position = GetRandomPlatformPosition();
			redPlatform.GetComponent<SpriteRenderer>().color = Color.red;
			_redPlatforms.Add(Instantiate(redPlatform));

			var bluePlatform = set.Item2;
			bluePlatform.transform.position = GetRandomPlatformPosition();
			bluePlatform.GetComponent<SpriteRenderer>().color = Color.blue;
			_bluePlatforms.Add(Instantiate(bluePlatform));
		}
	}


	
	private IEnumerator SetupGame()
	{	
		_objCannon.SetActive(false);
		_btnCheck.SetActive(false);
		
		StartCoroutine(ShowImageUntilTapped(_sprRedTeamTap));
		while (_coroutineRunning)
			yield return null;

		StartCoroutine(ShowImageForSeconds(_sprRedTeamArrange, 2.0f));
		while (_coroutineRunning)
			yield return null;
		
		ResetPlatforms();

		foreach (var platform in _redPlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeIn);
			platform.GetComponent<DraggableObject>().Draggable = true;
		}

		_btnCheck.SetActive(true);
		while (_btnCheck.activeInHierarchy)
		{
			yield return null;
		}

		foreach (var platform in _redPlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeOut);
			platform.GetComponent<DraggableObject>().Draggable = false;
		}

		StartCoroutine(ShowImageUntilTapped(_sprBlueTeamTap));
		while (_coroutineRunning)
			yield return null;

		StartCoroutine(ShowImageForSeconds(_sprBlueTeamArrange, 2.0f));
		while (_coroutineRunning)
			yield return null;
		
		foreach(var platform in _bluePlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeIn);
			platform.GetComponent<DraggableObject>().Draggable = true;
		}
		
		_btnCheck.SetActive(true);
		while (_btnCheck.activeInHierarchy)
		{
			yield return null;
		}
		
		foreach (var platform in _bluePlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeOut);
			platform.GetComponent<DraggableObject>().Draggable = false;
		}
		
		_btnCheck.SetActive(true);
		while (_btnCheck.activeInHierarchy)
		{
			yield return null;
		}

		foreach (var platform in _bluePlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeIn);
		}
		
		foreach (var platform in _redPlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeIn);
		}

		_objCannon.SetActive(true);
		active	 = true;
	}
	
}
