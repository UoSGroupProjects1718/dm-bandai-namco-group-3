using UnityEngine;

public class MusicInitiator : MonoBehaviour
{
	[SerializeField] private AudioClip _menuMusic;
	
	private void Start () {
		if(AudioManager.Instance != null)
			AudioManager.Instance.Play(_menuMusic);
	}	
}
