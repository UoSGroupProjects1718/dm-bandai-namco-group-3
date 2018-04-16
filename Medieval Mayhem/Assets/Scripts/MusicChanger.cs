using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] private AudioClip _music;

    private void Awake()
    {
        AudioManager.Instance.Play(_music);
    }

}
