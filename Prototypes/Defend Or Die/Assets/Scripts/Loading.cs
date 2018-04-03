using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _statusText;
    
    private AsyncOperation _asyncOperation;

    private void Start()
    {
        StartCoroutine(LoadGame());
    }
    
    private IEnumerator LoadGame()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(1);
        _asyncOperation.allowSceneActivation = false;

        while (_slider.value < 0.9f)
        {
            _slider.value = _asyncOperation.progress;
            _statusText.text = "Loading cannons... (" + (int)((_asyncOperation.progress + 0.1) * 100) + "%)";
            if (_slider.value >= 0.9f)
            {
                _slider.value = 1.0f;
                _asyncOperation.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
    }
    
}
