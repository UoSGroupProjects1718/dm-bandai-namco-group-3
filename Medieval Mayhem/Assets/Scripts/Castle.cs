using System.Collections;
using System.Collections.Generic;
using Exploder2D;
using UnityEngine;

public class Castle : MonoBehaviour
{
	public static string Tag = "Castle";

	public bool Exploded;
	
	[SerializeField] private GameObject _explosionAnimation;

	private void OnTriggerEnter2D(Collider2D collider2D)
	{
		if (collider2D.gameObject.CompareTag(Bomb.Tag))
		{
			GetComponent<Exploder2DObject>().Explode();
			_explosionAnimation.SetActive(true);
			Destroy(collider2D.gameObject);
			Exploded = true;
		}
	}
}
