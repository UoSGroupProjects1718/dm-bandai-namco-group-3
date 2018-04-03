using UnityEngine;

public class StoneButtonPressedEvent : ButtonPressedEvent
{
	[SerializeField] private GameManager _gameManager;
	
	[SerializeField] private GameObject _materialPanel;
	[SerializeField] private GameObject _stonePanel;
	
	[SerializeField] private GameObject _redStonePlatforms;
	[SerializeField] private GameObject _blueStonePlatforms;

	protected override void ButtonAction()
	{
		_materialPanel.SetActive(false);
		switch (_gameManager.ActivePlayer.Team)
		{
			case GameManager.PlayerData.PlayerTeam.Red:
				_redStonePlatforms.SetActive(true);
				break;
			case GameManager.PlayerData.PlayerTeam.Blue:
				_blueStonePlatforms.SetActive(true);
				break;
		}
		_stonePanel.SetActive(true);
	}
}
