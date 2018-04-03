using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMaterial : MonoBehaviour
{
    public string Name;
    public int Strength;
    public float Bounciness;
    public float Friction;

    public AudioClip HitSound;
    public AudioClip BreakSound;
}