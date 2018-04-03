using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class PlatformTwo : MonoBehaviour
{
	[Tooltip("Ignore fingers with StartedOverGui?")]
	public bool IgnoreGuiFingers = true;

	[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
	public int RequiredFingerCount;

	[Tooltip("Does translation require an object to be selected?")]
	public LeanSelectable RequiredSelectable;
	
	[Tooltip("The rotation axis used for non-relative rotations")]
	public Vector3 RotateAxis = Vector3.forward;

	[Tooltip("Should the rotation be performanced relative to the finger center?")]
	public bool Relative;

	[Tooltip("The camera the translation will be calculated using (None = MainCamera)")]
	public Camera Camera;
		
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
		// If we require a selectable and it isn't selected, cancel the translation
		if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
			return;

		// Obtain the fingers we want to use
		var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount, RequiredSelectable);

		// Calculate the screenDelta value based on these fingers
		var screenDelta = LeanGesture.GetScreenDelta(fingers);
		
		// Calculate the rotation values based on these fingers
		var center  = LeanGesture.GetScreenCenter(fingers);
		var degrees = LeanGesture.GetTwistDegrees(fingers);

		// Perform the rotation
		Rotate(center, degrees);

		// Perform the translation
		Translate(screenDelta);	
	}
	
	private void Rotate(Vector3 center, float degrees)
	{
		if (Relative == true)
		{
			// Make sure the camera exists
			var camera = LeanTouch.GetCamera(Camera, gameObject);

			if (camera != null)
			{
				// World position of the reference point
				var worldReferencePoint = camera.ScreenToWorldPoint(center);

				// Rotate the transform around the world reference point
				transform.RotateAround(worldReferencePoint, camera.transform.forward, degrees);
			}
		}
		else
		{
			transform.rotation *= Quaternion.AngleAxis(degrees, RotateAxis);
		}
	}
		
	protected virtual void Translate(Vector2 screenDelta)
	{
		// Make sure the camera exists
		var camera = LeanTouch.GetCamera(Camera, gameObject);
		
		if (camera == null) return;
		
		// Screen position of the transform
		var screenPosition = camera.WorldToScreenPoint(transform.position);
				
		// Add the deltaPosition
		screenPosition += (Vector3)screenDelta;

		// Perform the translation
		transform.position = camera.ScreenToWorldPoint(screenPosition);
	}
}