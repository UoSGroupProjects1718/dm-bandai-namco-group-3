using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class PlatformOOB : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collider2D)
	{
		Debug.Log("Trigger Enter");
		if (collider2D.gameObject.layer == LayerMask.NameToLayer("OOB"))
		{
			Debug.Log("HIT OOB");
			GetComponent<Transform>().position = Vector2.zero;
			FindObjectOfType<LeanSelect>().DeselectAll();
			//GetComponent<LeanSelectable>().Deselect();
		}	
	}
	
}
