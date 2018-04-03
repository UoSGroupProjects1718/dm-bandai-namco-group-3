using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent((typeof(Platform)))]
public class PlatformVisualisation : MonoBehaviour
{
    public List<DurabilityVisualisation> DurabilityVisualisations = new List<DurabilityVisualisation>();
    
    [Serializable]
    public struct DurabilityVisualisation {
        public int Strength;
        public Sprite Sprite;
    }

    private SpriteRenderer _spriteRenderer;
    private Platform _platform;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _platform = GetComponent<Platform>();
    }

    private void Update()
    {
        foreach (var durabilityVisualisation in DurabilityVisualisations)
        {
            if (durabilityVisualisation.Strength == _platform.Strength)
            {
                _spriteRenderer.sprite = durabilityVisualisation.Sprite;
                break;
            }   
        }
    }
}