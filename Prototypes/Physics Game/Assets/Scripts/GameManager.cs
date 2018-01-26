using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance
	{
		get { return _instance ?? (_instance = GameObject.FindObjectOfType<GameManager>()); }
	}
	
	private static GameManager _instance;
	
	private Team _activeTeam;
	
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		SetupGame();
	}

	private void SetupGame()
	{
		Debug.Log("Setting up  game...");
		_activeTeam = Random.Range(0, 2) == 0 ? Team.Red : Team.Green;
		Debug.LogFormat("Team {0} starts...", _activeTeam);
	}
	
}
