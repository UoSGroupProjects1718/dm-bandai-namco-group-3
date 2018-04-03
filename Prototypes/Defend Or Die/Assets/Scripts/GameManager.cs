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
	// Platform spawn coordinate range
	
	private const float PlatformPositionMinX = -80.0f;
	private const float PlatformPositionMaxX = 80.0f;
	private const float PlatformPositionMinY = 0.0f;
	private const float PlatformPositionMaxY = 40.0f;

	// Image viewer animator states
	
	private const string ImageAnimState = "State";
	
	private const int ImageAnimStateIdle = 0;
	private const int ImageAnimStateAppear = 1;
	private const int ImageAnimStateDisappear = 2;
	private const int ImageAnimStateTeamWins = 3;
	
	// Platform animator states
	private const string PlatformAnimState = "State";

	private const int PlatformAnimStateHidden = 0;
	private const int PlatformAnimStateFadeIn = 1;
	private const int PlatformAnimStateFadeOut = 2;


	[SerializeField] private GameObject _btnCheck;
	[SerializeField] private GameObject _objCannon;
	[SerializeField] private PlatformCollection _platformCollection;
	[SerializeField] private GameObject _imageViewer;
	[SerializeField] private Sprite _sprBothTeamsReady;
	[SerializeField] private Sprite _sprRedTeamWin;
	[SerializeField] private Sprite _sprRedTeamArrange;
	[SerializeField] private Sprite _sprRedTeamTap;
	[SerializeField] private Sprite _sprBlueTeamWin;
	[SerializeField] private Sprite _sprBlueTeamArrange;
	[SerializeField] private Sprite _sprBlueTeamTap;
	
	private readonly List<GameObject> _redPlatforms = new List<GameObject>();
	private readonly List<GameObject> _bluePlatforms = new List<GameObject>();

	private bool _coroutineRunning;
	private bool _active;
	private bool _cannonFired;

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
		while (imageAnimator.GetInteger(ImageAnimState) != ImageAnimStateIdle)
			yield return null;		
		
		// Set the sprite of the image viewer and begin the appear animation.
		_imageViewer.GetComponent<SpriteRenderer>().sprite = sprite;
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateAppear);
		
		// Wait for the appear animation to complete
		yield return new WaitForSeconds(_imageViewer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
		
		// Wait for the specified period.
		yield return new WaitForSeconds(seconds);
		
		// Start the disappear animation and wait for it to complete.
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateDisappear);
		yield return new WaitForSeconds(_imageViewer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
		
		// Return the image viewer to its idle state.
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateIdle);
		
		_coroutineRunning = false;
	}

	private IEnumerator ShowImageUntilTapped(Sprite sprite)
	{
		_coroutineRunning = true;
		
		// If the animator is currently busy, wait for it to return to the idle state.
		var imageAnimator = _imageViewer.GetComponent<Animator>();		
		while (imageAnimator.GetInteger(ImageAnimState) != ImageAnimStateIdle)
			yield return null;		
		
		// Set the sprite of the image viewer and begin the appear animation.
		_imageViewer.GetComponent<SpriteRenderer>().sprite = sprite;
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateAppear);
		
		// Wait for the appear animation to complete
		yield return new WaitForSeconds(_imageViewer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
		
		// Wait until the screen is tapped.
		while (Input.touchCount == 0)
			yield return null;
		
		// Start the disappear animation and wait for it to complete.
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateDisappear);
		yield return new WaitForSeconds(_imageViewer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
		
		// Return the image viewer to its idle state.
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateIdle);

		_coroutineRunning = false;
	}
	
	
	private void ShowImage(Sprite image)
	{
		_imageViewer.GetComponent<SpriteRenderer>().sprite = image;
		_imageViewer.GetComponent<Animator>().SetInteger("State", 1);
	}


	private void Update()
	{
		Debug.LogFormat("Coroutine: {0}, Anim State: {1}", _coroutineRunning, _imageViewer.GetComponent<Animator>().GetInteger(ImageAnimState));
		Debug.LogFormat("Touches: {0}", Input.touchCount);
		
		if (Input.GetKey(KeyCode.Space))
		{
			SceneManager.LoadScene(0, LoadSceneMode.Single);
		}
		
		if (!_active) return;

		if (!_cannonFired && GameObject.FindGameObjectWithTag(Bomb.Tag))
		{
			_cannonFired = true;
		}

		if (_cannonFired)
		{
			if (!GameObject.FindGameObjectWithTag(Bomb.Tag))
			{
				_imageViewer.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Red Castle Explosion") != null
					? _sprBlueTeamWin
					: _sprRedTeamWin;

				foreach (var platform in _redPlatforms)
					platform.GetComponent<Exploder2DObject>().Explode();
				foreach (var platform in _bluePlatforms)
					platform.GetComponent<Exploder2DObject>().Explode();
				
				_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateTeamWins);
				_active = false;

				StartCoroutine(ResetGame());
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

	private static IEnumerator ResetGame()
	{
		yield return new WaitForSeconds(5.0f);
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}
	
	private IEnumerator SetupGame()
	{	
		// Ensure that the check button and cannon are disabled
		_objCannon.SetActive(false);
		_btnCheck.SetActive(false);
		
		// Ensure the image viewer is in the idle state
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateIdle);
		
		// Obtain and place a random set of platforms
		ResetPlatforms();
		
		// Small delay before text pops up
		yield return new WaitForSeconds(1.0f);
		
		// Prompt the red team to tap the screen
		StartCoroutine(ShowImageUntilTapped(_sprRedTeamTap));
		while (_coroutineRunning)
			yield return null;

		// Prompt the red team to arrange their platforms
		StartCoroutine(ShowImageForSeconds(_sprRedTeamArrange, 2.0f));
		while (_coroutineRunning)
			yield return null;
		
		// Fade in the red teams platforms and make them draggable
		foreach (var platform in _redPlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeIn);
			platform.GetComponent<DraggableObject>().Draggable = true;
		}

		// Enable the check button and wait for it to be pressed
		_btnCheck.SetActive(true);
		while (_btnCheck.activeInHierarchy)
			yield return null;

		// Fade out the red teams platforms
		foreach (var platform in _redPlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeOut);
			platform.GetComponent<DraggableObject>().Draggable = false;
		}

		// Prompt the blue team to tap the screen
		StartCoroutine(ShowImageUntilTapped(_sprBlueTeamTap));
		while (_coroutineRunning)
			yield return null;

		// Prompt the blue team to arrange their platforms
		StartCoroutine(ShowImageForSeconds(_sprBlueTeamArrange, 2.0f));
		while (_coroutineRunning)
			yield return null;
		
		// Fade in the blue teams platforms and make them draggable
		foreach(var platform in _bluePlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeIn);
			platform.GetComponent<DraggableObject>().Draggable = true;
		}
		
		// Enable the check button and wait for it to be pressed
		_btnCheck.SetActive(true);
		while (_btnCheck.activeInHierarchy)
			yield return null;
		
		// Fade out the blue teams platforms		
		foreach (var platform in _bluePlatforms)
		{
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeOut);
			platform.GetComponent<DraggableObject>().Draggable = false;
		}

		// Fade in the both teams ready text
		_imageViewer.GetComponent<SpriteRenderer>().sprite = _sprBothTeamsReady;
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateAppear);
		
		// Enable the check button and wait for it to be pressed
		_btnCheck.SetActive(true);
		while (_btnCheck.activeInHierarchy)
			yield return null;
		
		// Fade in the both teams ready text
		_imageViewer.GetComponent<SpriteRenderer>().sprite = _sprBothTeamsReady;
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateDisappear);
		
		// Wait for the appear animation to complete
		yield return new WaitForSeconds(_imageViewer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
		
		// Reset the image viewer to the idle state
		_imageViewer.GetComponent<Animator>().SetInteger(ImageAnimState, ImageAnimStateIdle);

		// Fade in both teams platforms
		foreach (var platform in _bluePlatforms)
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeIn);
		foreach (var platform in _redPlatforms)
			platform.GetComponent<Animator>().SetInteger(PlatformAnimState, PlatformAnimStateFadeIn);

		// Enable the cannon and stat the game
		_objCannon.SetActive(true);
		_active = true;
	}
	
}
