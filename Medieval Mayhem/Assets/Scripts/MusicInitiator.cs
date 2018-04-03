using UnityEngine;

public class MusicInitiator : MonoBehaviour
{
	[SerializeField] private AudioClip _menuMusic;
	
	private void Start () {
		AudioManager.Instance.Play(_menuMusic);
	}	
}
