using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TarManState
{
	public bool isCurrentState;
	[HideInInspector] public TarManController tarMan;

	public virtual void Awake() { }

	public virtual void Start() { }

	public virtual void Enter()
	{
		if (tarMan == null)
		{
			Debug.LogError("!!! TarMan reference not set on state of type " + GetType().Name + "!!!");
		}
	}

	public virtual void Update() { }

	public virtual void FixedUpdate() { }

	public virtual void Exit() { }

	public virtual void OnValidate()
	{
		if (Application.isPlaying)
		{
			if (isCurrentState && tarMan.currentState != this)
			{
				tarMan.TransitionState(this);
			}
		}
		else
		{
			isCurrentState = false;
		}
	}
}
