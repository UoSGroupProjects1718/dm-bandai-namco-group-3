using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject _winTextObject;
	[SerializeField] private Sprite _redTeamWinSprite;
	[SerializeField] private Sprite _blueTeamWinSprite;

	private bool active;
	private bool potato;

	public static GameManager Instance
	{
		get { return _instance ?? (_instance = GameObject.FindObjectOfType<GameManager>()); }
	}
	
	private static GameManager _instance;
	
	private Team _activeTeam;

	private void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			Debug.Log("SPACEMAN");
			SceneManager.LoadScene(0, LoadSceneMode.Single);
		}
		if (!active) return;

		if (!potato && GameObject.FindGameObjectWithTag(Bomb.Tag))
		{
			potato = true;
		}

		if (potato)
		{
			if (!GameObject.FindGameObjectWithTag(Bomb.Tag))
			{
				_winTextObject.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Red Castle Explosion") != null
					? _blueTeamWinSprite
					: _redTeamWinSprite;
				_winTextObject.SetActive(true);
				GameObject.Find("Platforms").SetActive(false);
				active = false;
			}
		}
	}
	
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		SetupGame();
	}

	private void SetupGame()
	{
		active = true;
		Debug.Log("Setting up  game...");
		_activeTeam = Random.Range(0, 2) == 0 ? Team.Red : Team.Green;
		Debug.LogFormat("Team {0} starts...", _activeTeam);
	}
	
}
