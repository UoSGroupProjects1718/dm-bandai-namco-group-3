using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
	[SerializeField] private AudioClip _musicClip;
	
	private void Start () {
		MusicManager.play(_musicClip);
	}
	
	
	
}
