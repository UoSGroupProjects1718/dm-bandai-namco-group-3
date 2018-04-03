using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Scroll : MonoBehaviour
{
    private const string AnimState = "State";
    private const int AnimStateRise = 0;
    private const int AnimStateFall = 1;
    private const int AnimStateRiseFully = 3;



    private Animator _animator;

    [SerializeField] private GameManager _gameManager;

    [SerializeField] private GameObject _materialPanel;
    [SerializeField] private GameObject _stonePanel;
    [SerializeField] private GameObject _woodPanel;
    [SerializeField] private GameObject _glassPanel;
    
    

    [SerializeField] private List<GameObject> _unplacedPlatforms;

    private void HideUnplacedPlatforms()
    {
        foreach(var platform in _unplacedPlatforms)
            platform.SetActive(false);
    }
    
    private void Update()
    {
        if (_gameManager.ActivePlayer.GlassQuantity == 0 && _glassPanel.activeInHierarchy)
        {
            _glassPanel.SetActive(false);
            _materialPanel.SetActive(true);
            HideUnplacedPlatforms();
        }
        else if (_gameManager.ActivePlayer.WoodQuantity == 0 && _woodPanel.activeInHierarchy)
        {
            _woodPanel.SetActive(false);
            _materialPanel.SetActive(true);
            HideUnplacedPlatforms();
        }
        else if (_gameManager.ActivePlayer.StoneQuantity == 0 && _stonePanel.activeInHierarchy)
        {
            _stonePanel.SetActive(false);
            _materialPanel.SetActive(true);
            HideUnplacedPlatforms();
        }
    }

    public void ShowMaterialPanel()
    {
        _stonePanel.SetActive(false);
        _woodPanel.SetActive(false);
        _glassPanel.SetActive(false);
        HideUnplacedPlatforms();
        _materialPanel.SetActive(true);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void HidePeak()
    {
        _animator.SetInteger(AnimState, AnimStateRise);
    }

    public void DropdownPeak()
    {
        _animator.SetInteger(AnimState, AnimStateFall);
    }
    
    public void Dropdown()
    {
        _animator.SetInteger(AnimState, AnimStateFall);
    }

    public void Hide()
    {
        _animator.SetInteger(AnimState, AnimStateRiseFully);
    }
}
