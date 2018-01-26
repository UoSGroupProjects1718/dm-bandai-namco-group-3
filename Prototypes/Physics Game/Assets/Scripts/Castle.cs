using System.Collections;
using System.Collections.Generic;
using Exploder2D;
using UnityEngine;

public class Castle : MonoBehaviour
{
	public static string Tag = "Castle";
	
	[SerializeField] private GameObject _explosionAnimation;

	private void OnCollisionEnter2D(Collision2D collision2D)
	{
		if (collision2D.gameObject.CompareTag(Bomb.Tag))
		{
			GetComponent<Exploder2DObject>().Explode();
			_explosionAnimation.SetActive(true);
		}
	}
}
