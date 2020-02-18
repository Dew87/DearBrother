using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWhipState : PlayerGrappleBaseState
{
	public override void Awake()
	{
		base.Awake();
	}
	public override void Start()
	{
		base.Start();
	}
	public override void Enter()
	{
		base.Enter();
		player.grappleDetection.grapplePointBehaviour.UseGrapple();
		player.grappleDetection.ReleaseGrapplePoint();
		if (player.previousState != player.whipState && player.previousState != player.swingState)
		{
			player.TransitionState(player.previousState);
		}
		else
		{
			player.TransitionState(player.fallingState);
		}
	}
	public override void Update()
	{
		base.Update();
	}
	public override void FixedUpdate()
	{
		base.FixedUpdate();
	}
	public override void Exit()
	{
		base.Exit();
	}
	public override void OnValidate()
	{
		base.OnValidate();
	}
}
