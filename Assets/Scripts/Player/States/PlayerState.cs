using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerState
{
	[Tooltip("Read only, only here for debug purposes. Changing this does nothing.")]
	public bool isCurrentState;
	[HideInInspector] public PlayerController player;

	public virtual void Start()
	{
		if (player == null)
		{
			Debug.LogError("Player reference not set on state"); 
		}
	}

	public virtual void Update()
	{
	}

	public virtual void FixedUpdate()
	{

	}

	public virtual void Enter()
	{

	}

	public virtual void Exit()
	{

	}

	public virtual void OnValidate()
	{
	}
}
