using System.Collections;
using UnityEngine;

namespace Scenes.Main.Scripts
{
	public class DeactivateOnCollision : MonoBehaviour
	{
		[SerializeField] private string _tag;
		[SerializeField] private float _timeBeforeDeactivation;

		private bool _deactivating;

		private IEnumerator Deactivate()
		{
			yield return new WaitForSeconds(_timeBeforeDeactivation);
			gameObject.SetActive(false);
		}

		private void OnCollisionEnter2D(Collision2D collision2D)
		{
			if (collision2D.gameObject.CompareTag(_tag) && !_deactivating)
			{
				_deactivating = true;
				StartCoroutine(Deactivate());
			}
		}
	}
}
