using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGlidingState : PlayerState
{
	public float descendSpeed = 2;
	public float verticalDeceleration = 20f;
	public float horizontalAcceleration = 20f;
	public float horizontalDeceleration = 10f;

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		player.MoveHorizontally(player.isSprintInputHeld ? player.walkingState.runSpeed : player.walkingState.walkSpeed, horizontalAcceleration, horizontalDeceleration);

		float delta = player.velocity.y > -descendSpeed ? player.fallingState.gravity : verticalDeceleration;
		player.velocity.y = Mathf.MoveTowards(player.velocity.y, -descendSpeed, delta * Time.deltaTime);
	}

	public override void Update()
	{
		base.Update();

		if (!player.isJumpInputHeld)
		{
			player.TransitionState(player.fallingState);
		}

		if (player.CheckBoxcast(Vector2.down))
		{
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
}
