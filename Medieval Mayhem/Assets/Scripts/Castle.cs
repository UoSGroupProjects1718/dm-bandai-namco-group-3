using System.Collections;
using System.Collections.Generic;
using Exploder2D;
using UnityEngine;

public class Castle : MonoBehaviour
{
	public static string Tag = "Castle";

	public bool Exploded;
	
	[SerializeField] private GameObject _explosionAnimation;
	[SerializeField] private AudioClip _sfxExplosion;

	private void OnTriggerEnter2D(Collider2D collider2D)
	{
		if (collider2D.gameObject.CompareTag(Bomb.Tag))
		{
			AudioSource.PlayClipAtPoint(_sfxExplosion, Vector2.zero, 1.0f);
			GetComponent<Exploder2DObject>().Explode();
			_explosionAnimation.SetActive(true);
			if (AudioManager.Instance != null)
				AudioManager.Instance.Stop(0.0f);
			Destroy(collider2D.gameObject);
			Exploded = true;
		}
	}
}
