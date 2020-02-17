using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGrappleBaseState : PlayerState
{
	public float grappleLength { get; protected set; }

	protected float maxGrappleLength;
	public override void Enter()
	{
		base.Enter();
		maxGrappleLength = player.grappleDetection.detectionRadius;
		grappleLength = Vector2.Distance(player.transform.position, player.grappleDetection.currentGrapplePoint.transform.position);
		if (grappleLength > maxGrappleLength)
		{
			grappleLength = maxGrappleLength;
		}
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
		if (player.isJumpInputPressedBuffered)
		{
			if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Swing || player.CheckOverlaps(Vector2.down))
			{
				player.grappleDetection.ReleaseGrapplePoint();
				player.TransitionState(player.jumpingState);
				return;
			}
			else if (player.doesDoubleJumpRemain)
			{
				player.grappleDetection.ReleaseGrapplePoint();
				player.TransitionState(player.doubleJumpingState);
				return;
			}
		}

	}
	public override void OnValidate()
	{
		base.OnValidate();
	}
}
