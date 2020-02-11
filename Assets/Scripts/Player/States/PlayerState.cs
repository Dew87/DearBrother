using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerState
{
	public bool isCurrentState;
	[HideInInspector] public PlayerController player;

	public virtual void Awake() { }

	public virtual void Start() { }

	public virtual void Enter()
	{
		if (player == null)
		{
			Debug.LogError("!!! Player reference not set on state of type " + GetType().Name + "!!!");
		}
	}

	public virtual void Update() { }

	public virtual void FixedUpdate() { }

	public virtual void Exit() { }

	public virtual void OnValidate()
	{
		if (Application.isPlaying)
		{
			if (isCurrentState && player.currentState != this)
			{
				player.TransitionState(this);
			}
		}
		else
		{
			isCurrentState = false;
		}
	}
}
