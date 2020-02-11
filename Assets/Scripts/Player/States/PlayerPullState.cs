using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPullState : PlayerGrappleBaseState
{
	public float speed = 2.3f;
	public float acceleration = 40f;
	public float deceleration = 30f;
	[Tooltip("How far the player can go before the pull cancels")]
	public float stretchTolerance = 1f;
	public bool isStretchToleranceMultiplier = false;
	public float pullspeed = 1f;
	public override void Enter()
	{
		base.Enter();
		player.ResetGrappleInputBuffer();
	}
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		player.velocity.y = Mathf.MoveTowards(player.velocity.y, -player.fallingState.maxFallSpeed, player.fallingState.gravity * Time.deltaTime);
		bool isPulling;
		Vector3 grapplePos = player.grappleDetection.currentGrapplePoint.transform.position;
		if (player.horizontalInputAxis > 0)
		{
			isPulling = grapplePos.x < player.transform.position.x ? true : false;
		}
		else
		{
			isPulling = grapplePos.x > player.transform.position.x ? true : false;
		}

		float currentSpeed = isPulling && Vector2.Distance(player.transform.position, grapplePos) > grappleLength ? speed : player.walkingState.speed;
		player.MoveHorizontally(currentSpeed, acceleration, deceleration);

		if (isPulling && Vector2.Distance(player.transform.position, grapplePos) > grappleLength)
		{
			player.grappleDetection.grapplePointBehaviour.rb2d.velocity = Vector2.MoveTowards(player.grappleDetection.grapplePointBehaviour.rb2d.velocity, player.rb2d.velocity, pullspeed * Time.deltaTime);
		}
	}
	public override void Update()
	{
		base.Update();
		if (player.grappleDetection.currentGrapplePoint != null)
		{
			if (player.isGrappleInputPressedBuffered)
			{
				if (player.grappleDetection.nextGrapplePoint != null)
				{
					player.grappleDetection.ReleaseGrapplePoint();
					player.grappleDetection.SwitchCurrentNext();
					if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Swing)
					{
						player.TransitionState(player.swingState);
					}
					else if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Pull)
					{
						player.TransitionState(player.pullState);
					}
					else
					{
						player.TransitionState(player.whipState);
					}
					return;
				}
				else
				{
					player.grappleDetection.ReleaseGrapplePoint();
					if (!player.CheckOverlaps(Vector2.down))
					{
						player.TransitionState(player.fallingState);
						return;
					}
					else
					{
						player.TransitionState(player.walkingState);
						return;
					}
				}
			}
			else if (Vector2.Distance(player.transform.position, player.grappleDetection.currentGrapplePoint.transform.position) > (isStretchToleranceMultiplier ? grappleLength * stretchTolerance : grappleLength + stretchTolerance))
			{
				player.grappleDetection.ReleaseGrapplePoint();
				if (!player.CheckOverlaps(Vector2.down))
				{
					player.TransitionState(player.fallingState);
					return;
				}
				else
				{
					player.TransitionState(player.walkingState);
					return;
				}
			}
		}
	}
}
