using System.Collections.Generic;
using UnityEngine;

namespace Lean.Touch
{
	// This script allows you to transform the current GameObject
	public class LeanTranslate : MonoBehaviour
	{
		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreGuiFingers = true;

		[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
		public int RequiredFingerCount;

		[Tooltip("Does translation require an object to be selected?")]
		public LeanSelectable RequiredSelectable;

		[Tooltip("The camera the translation will be calculated using (None = MainCamera)")]
		public Camera Camera;
		
		[SerializeField] private LayerMask _outOfBoundsLayerMask;

		private Vector3 pos;
		private bool collided;
		
		private List<Vector3> positions = new List<Vector3>();

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Start();
		}
#endif

		protected virtual void Start()
		{
			if (RequiredSelectable == null)
			{
				RequiredSelectable = GetComponent<LeanSelectable>();
			}
		}

		protected virtual void Update()
		{

			// If we require a selectable and it isn't selected, cancel translation
			if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
			{
				return;
			}

			// Get the fingers we want to use
			var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount, RequiredSelectable);

			// Calculate the screenDelta value based on these fingers
			var screenDelta = LeanGesture.GetScreenDelta(fingers);

			// Perform the translation
			Translate(screenDelta);

			Debug.Log(collided);
			
			if (!collided)
			{
				positions.Add(transform.position);
			}
			else
			{
				transform.position = new Vector3(0, 3.8f, 1);
				//GameObject.Find("LeanSelect").GetComponent<LeanSelect>().DeselectAll();
			}

			
		}



		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.layer == 8)
			{
				collided = true;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.layer == 8)
			{
				collided = false;
				positions.Clear();
			}	
		}
		
		protected virtual void Translate(Vector2 screenDelta)
		{
			// Make sure the camera exists
			var camera = LeanTouch.GetCamera(Camera, gameObject);

			if (camera != null)
			{
				// Screen position of the transform
				var screenPosition = camera.WorldToScreenPoint(transform.position);
				
				// Add the deltaPosition
				screenPosition += (Vector3)screenDelta;
				
				if(!collided)
				transform.position = camera.ScreenToWorldPoint(screenPosition);
			}
			
		}
	}
}