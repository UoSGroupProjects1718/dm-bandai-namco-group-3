using UnityEngine;

public class WoodButtonPressedEvent : ButtonPressedEvent
{
    [SerializeField] private GameManager _gameManager;
	
    [SerializeField] private GameObject _materialPanel;
    [SerializeField] private GameObject _woodPanel;
	
    [SerializeField] private GameObject _redWoodPlatforms;
    [SerializeField] private GameObject _blueWoodPlatforms;

    protected override void ButtonAction()
    {
        _materialPanel.SetActive(false);
        switch (_gameManager.ActivePlayer.Team)
        {
            case GameManager.PlayerData.PlayerTeam.Red:
                _redWoodPlatforms.SetActive(true);
                break;
            case GameManager.PlayerData.PlayerTeam.Blue:
                _blueWoodPlatforms.SetActive(true);
                break;
        }
        _woodPanel.SetActive(true);
    }
}
