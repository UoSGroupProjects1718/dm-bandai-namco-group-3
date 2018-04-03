using System;
using Lean.Touch;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour 
{
	[Tooltip("Ignore fingers with StartedOverGui?")]
	public bool IgnoreGuiFingers = true;

	[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
	public int RequiredFingerCount;

	[Tooltip("Does translation require an object to be selected?")]
	public LeanSelectable RequiredSelectable;

	public bool IsDraggable;
	public bool BeenDragged;

	private static int TopLayer = 1000;
	
	protected virtual void Update()
	{
		if (!IsDraggable || (RequiredSelectable != null && RequiredSelectable.IsSelected == false))
			return;

		var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount, RequiredSelectable);
		var screenDelta = LeanGesture.GetScreenDelta(fingers);

		Translate(screenDelta);		
	}
	
	protected virtual void Translate(Vector2 screenDelta)
	{
		var spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer != null)
		{
			spriteRenderer.sortingOrder = ++TopLayer;
		}
		transform.position = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(transform.position) + (Vector3)screenDelta);
		BeenDragged = true;
	}
}
	