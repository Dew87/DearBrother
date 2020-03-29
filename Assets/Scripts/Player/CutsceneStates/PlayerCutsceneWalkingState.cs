using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCutsceneWalkingState : PlayerState
{
	[HideInInspector] public float speed;
	[HideInInspector] public Vector2 targetPosition;
	[HideInInspector] public bool shouldStopInstantly = false;
	[HideInInspector] public bool isGliding = false;

	private bool hasReachedTarget = false;
	private bool isMovingRight = false;

	public override void Enter()
	{
		base.Enter();
		player.IsInCutscene = true;
		hasReachedTarget = false;
		player.playerAnimator.SetBool("Moving", true);
		isMovingRight = targetPosition.x > player.rb2d.position.x;
		player.spriteRenderer.flipX = !isMovingRight;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		bool grounded = player.CheckOverlaps(Vector2.down);

		player.playerAnimator.SetBool("Grounded", grounded);

		if (isGliding && !grounded)
		{
			player.playerAnimator.SetBool("Gliding", true);
			float descendSpeed = player.glidingState.descendSpeed;
			float acceleration = player.velocity.y > -descendSpeed ? player.fallingState.gravity : player.glidingState.verticalDeceleration;
			player.velocity.y = Mathf.MoveTowards(player.velocity.y, -player.glidingState.descendSpeed, acceleration * Time.deltaTime);
		}
		else
		{
			player.playerAnimator.SetBool("Gliding", false);
			if (player.velocity.y < 0)
			{
				player.soundManager.SamLandSound();
			}
			player.velocity.y = 0;
		}

		if (hasReachedTarget)
		{ 
			player.velocity.x = Mathf.MoveTowards(player.velocity.x, 0, player.walkingState.deceleration * Time.deltaTime);
			if (player.velocity.x == 0)
			{
				if (!isGliding || grounded)
				{
					player.TransitionState(player.cutsceneStandingState); 
				}
			}
		}
		else
		{
			player.velocity.x = Mathf.MoveTowards(player.velocity.x, isMovingRight ? speed : -speed, player.walkingState.acceleration * Time.deltaTime);

			float playerX = player.rb2d.position.x;
			if ((isMovingRight && playerX >= targetPosition.x) || (!isMovingRight && playerX <= targetPosition.x))
			{
				hasReachedTarget = true;
				if (shouldStopInstantly)
				{
					player.velocity = Vector2.zero;
					hasReachedTarget = true;
				}
			}
		}
	}

	public override void Exit()
	{
		base.Exit();
		player.velocity = Vector2.zero;
		player.IsInCutscene = false;
		player.playerAnimator.SetBool("Moving", false);
		player.playerAnimator.SetBool("Gliding", false);
		player.isFacingRight = isMovingRight;
	}
}
