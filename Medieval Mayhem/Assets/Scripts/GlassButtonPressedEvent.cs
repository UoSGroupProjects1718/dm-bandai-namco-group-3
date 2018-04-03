using UnityEngine;

public class GlassButtonPressedEvent : ButtonPressedEvent
{
    [SerializeField] private GameManager _gameManager;
	
    [SerializeField] private GameObject _materialPanel;
    [SerializeField] private GameObject _glassPanel;
	
    [SerializeField] private GameObject _redGlassPlatforms;
    [SerializeField] private GameObject _blueGlassPlatforms;

    protected override void ButtonAction()
    {
        _materialPanel.SetActive(false);
        switch (_gameManager.ActivePlayer.Team)
        {
            case GameManager.PlayerData.PlayerTeam.Red:
                _redGlassPlatforms.SetActive(true);
                break;
            case GameManager.PlayerData.PlayerTeam.Blue:
                _blueGlassPlatforms.SetActive(true);
                break;
        }
        _glassPanel.SetActive(true);
    }
}
