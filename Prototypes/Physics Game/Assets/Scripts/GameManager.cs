using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
	Red, Green
}

public class GameManager : MonoBehaviour {

	private static readonly Color TeamRedColour = new Color(255.0f / 255.0f, 105.0f / 255.0f, 97.0f / 255.0f);
	private static readonly Color TeamGreenColour = new Color(119.0f / 255.0f, 211.0f / 255.0f, 119.0f / 255.0f);

	private Team _activeTeam;

	[SerializeField] private Fish _fish;
	
	private static GameManager _instance;
	
	public static GameManager Instance => _instance ?? (_instance = FindObjectOfType<GameManager>());

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		SetupGame();
	}

	private void SetupGame()
	{
		Debug.Log("Setting up game...");
		_activeTeam = Random.Range(0, 2) == 0 ? Team.Red : Team.Green;
		Debug.LogFormat("Team {0} starts...", _activeTeam);
		_fish.SetColour(_activeTeam == Team.Red ? TeamRedColour : TeamGreenColour);
		_fish.SetPhysicsEnabled(true);
	}
	
}
