using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerFallingState : PlayerState
{
	public float maxFallSpeed = 10;
	public float windAcceleration = 60f;
	public float maxAscendSpeedInWind = 8f;
	public float maxHorizontalSpeedInWind = 10f;
	[Tooltip("If the player has fallen for at least this manys seconds when landing, landing lag occurs")]
	public float landingLagDurationThreshold = 12f;
	public float gravity = 51;
	public float acceleration = 18;
	public float deceleration = 20;

	private float landingLagTimer;
	
	public override void Enter()
	{
		base.Enter();
		player.playerAnimator.SetBool("Grounded", false);
		landingLagTimer = landingLagDurationThreshold;
	}

	public override void Exit()
	{
		base.Exit();

		player.ResetJumpGraceTimer();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

        player.MoveHorizontally(player.walkingState.speed, acceleration, player.isInWind ? deceleration / (Mathf.Abs(player.windSpeed.x) + 1) : deceleration);
		if (player.isInWind)
		{
			Vector2 currentWindSpeed = player.windSpeed * windAcceleration;
			if (currentWindSpeed.y - gravity > 0)
			{
				float deltaY = currentWindSpeed.y;
				player.velocity.y = Mathf.MoveTowards(player.velocity.y, maxAscendSpeedInWind, deltaY * Time.deltaTime);
			}
			else
			{
				float delta = Mathf.Abs(currentWindSpeed.y) + gravity;
				player.velocity.y = Mathf.MoveTowards(player.velocity.y, -maxFallSpeed, delta * Time.deltaTime);
			}
			player.velocity.x = Mathf.MoveTowards(player.velocity.x, currentWindSpeed.x > 0 ? maxHorizontalSpeedInWind : -maxHorizontalSpeedInWind, Mathf.Abs(currentWindSpeed.x) * Time.deltaTime );
		}
		else
		{
			player.velocity.y = Mathf.MoveTowards(player.velocity.y, -maxFallSpeed, gravity * Time.deltaTime);
		}
		if (landingLagTimer > 0)
		{
			landingLagTimer -= Time.deltaTime;
		}

		Collider2D ground = player.CheckOverlaps(Vector2.down);

        if (ground)
        {   
            if (landingLagTimer > 0)
            {
                if (player.velocity.x == 0)
                {
                    player.TransitionState(player.standingState);
					player.FindCorrectGroundDistance();
                }
                else
                {
                    player.TransitionState(player.walkingState);
					player.FindCorrectGroundDistance();
				}
			}
            else
            {
                player.TransitionState(player.landingLagState);
				player.FindCorrectGroundDistance();
			}
			return;
        }

		if (player.jumpGraceTimer > 0 && player.isJumpInputPressedBuffered)
		{
			player.TransitionState(player.jumpingState);
		}
		else if (player.hasDoubleJump && player.doesDoubleJumpRemain && player.isJumpInputPressedBuffered)
		{
			player.TransitionState(player.doubleJumpingState);
		}
		else if (player.isJumpInputHeld && player.hasFloat)
		{
			player.TransitionState(player.glidingState);
		}
		else if (player.isGrappleInputPressedBuffered && player.grappleDetection.currentGrapplePoint != null)
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

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
	}
}
