using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGlidingState : PlayerState
{
    public float descendSpeed = 2;
	public float maxAscendSpeedInWind = 4f;
	public float maxHorizontalSpeedInWind = 20f;
	public float verticalDeceleration = 20f;
    public float horizontalAcceleration = 20f;
    public float horizontalDeceleration = 10f;
	public override void FixedUpdate()
	{
		base.FixedUpdate();

        player.MoveHorizontally(player.walkingState.speed, horizontalAcceleration, horizontalDeceleration);
		if (player.isInWind)
		{
			player.velocity.x = Mathf.MoveTowards(player.velocity.x, maxHorizontalSpeedInWind, player.windSpeed.x * Time.deltaTime);
			player.velocity.y = Mathf.MoveTowards(player.velocity.y, maxAscendSpeedInWind, player.windSpeed.y * Time.deltaTime);
		}
		else
		{
			float delta = player.velocity.y > -descendSpeed ? player.fallingState.gravity : verticalDeceleration;
			player.velocity.y = Mathf.MoveTowards(player.velocity.y, -descendSpeed, delta * Time.deltaTime);
		}


		Collider2D roof = player.CheckOverlaps(Vector2.up);
		if (roof && player.velocity.y > 0)
		{
			player.velocity.y = 0;
		}

		Collider2D ground = player.CheckOverlaps(Vector2.down);
		if (ground)
		{
			if (ground.TryGetComponent<Bouncer>(out Bouncer bouncer))
			{
				bouncer.Bounce(player);
				return;
			}

			if (player.velocity.x == 0)
			{
				player.TransitionState(player.standingState);
			}
			else
			{
				player.TransitionState(player.walkingState);
			}
			return;
		}
	}

	public override void Update()
	{
		base.Update();

		if (!player.isJumpInputHeld)
		{
			player.TransitionState(player.fallingState);
		}


		if (player.isGrappleInputPressedBuffered && player.grappleDetection.currentGrapplePoint != null)
		{
			if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Swing)
			{
				player.TransitionState(player.swingState);
			}
			else if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Pull)
			{
				player.TransitionState(player.pullState);
			}
			else if (player.grappleDetection.grapplePointBehaviour.grappleType == GrapplePointBehaviour.GrappleType.Whip)
			{
				player.TransitionState(player.whipState);
			}
		}
	}
}
