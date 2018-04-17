using Lean.Touch;
using UnityEngine;

public class OOBArea : MonoBehaviour
{
	private const string AnimOOBState = "State";
	
	private const int AnimOOBStateAppear = 1;
	private const int AnimOOBStateDisappear = 2;

	private LeanSelect _leanSelect;
	private Animator _animator;
	private bool _hasAppearedOnce;

	private void Start ()
	{
		_leanSelect = GameObject.FindObjectOfType<LeanSelect>();
		_animator = GetComponent<Animator>();
	}

	private void Update ()
	{
		if (!_hasAppearedOnce && _leanSelect.CurrentSelectables.Count > 0)
		{
			_animator.SetInteger(AnimOOBState, AnimOOBStateAppear);
			_hasAppearedOnce = true;
		}
		
		_animator.SetInteger(AnimOOBState,
			_leanSelect.CurrentSelectables.Count == 0 ? AnimOOBStateDisappear : AnimOOBStateAppear);
	}
}
