using System;
using System.Collections;
using System.Collections.Generic;
using Exploder2D;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private const int SceneIndexGame = 1;
    private const float ProgressDone = 0.9f;
    
    [SerializeField] private Slider _slider;
    [SerializeField] private Exploder2DObject _bomb;
    [SerializeField] private GameObject _loadingObjects;
    [SerializeField] private GameObject _menuObjects;

    [SerializeField] private Text _log;

    [SerializeField] private MusicPlayer _musicPlayer;


    [SerializeField] private AudioClip _explosionSound;
    
    private AsyncOperation _asyncOperation;
    private Fader _fader;

    private void Start()
    {
        _fader = GetComponent<Fader>();
        StartCoroutine(LoadGame());
    }
    
    public void StartGame()
    {
        StartCoroutine(FadeAndStartGame());
    }
    
    public IEnumerator FadeAndStartGame()
    {
        _musicPlayer.StopMusic(1.0f);
        _fader.FadeOut();
        while (!_fader.HasFaded())
            yield return null;
        _asyncOperation.allowSceneActivation = true;
    }
    
    private IEnumerator LoadGame()
    {
        while (!GetComponent<Fader>().HasFaded())
            yield return null;
        
        _asyncOperation = SceneManager.LoadSceneAsync(SceneIndexGame);
        _asyncOperation.allowSceneActivation = false;

        while (_slider.value > _slider.minValue)
        {
            _log.text = "Loaded: " + _asyncOperation.progress + " | Slider: " + _slider.value;
            
            // Slider: 0.8
            // Progress: 0.6
            
            _slider.value -= Math.Min(_slider.value - _asyncOperation.progress, 0.01f);
                        
            if (_asyncOperation.progress >= ProgressDone && _slider.value <= _slider.minValue)
            {
                _slider.value = _slider.minValue;
                _slider.gameObject.SetActive(false);
                
                _bomb.Explode();
                GetComponent<AudioSource>().PlayOneShot(_explosionSound);
                
                _loadingObjects.SetActive(false);
                _menuObjects.SetActive(true);
                
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    

}
