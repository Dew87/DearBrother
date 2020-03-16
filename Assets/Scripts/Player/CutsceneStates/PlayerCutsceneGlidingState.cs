using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCutsceneGlidingState : PlayerState
{
	public override void Enter()
	{
		base.Enter();
		player.IsInCutscene = true;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (player.velocity.x == 0)
		{
			player.playerAnimator.SetBool("Moving", false);
		}
		else
		{
			player.velocity.x = Mathf.MoveTowards(player.velocity.x, 0, player.walkingState.deceleration * Time.deltaTime);
			player.playerAnimator.SetBool("Moving", true);
		}

		if (player.CheckOverlaps(Vector2.down))
		{
			player.velocity.y = 0;
			player.playerAnimator.SetBool("Grounded", true);
		}
		else
		{
			player.velocity.y = Mathf.MoveTowards(player.velocity.y, -player.fallingState.maxFallSpeed, player.fallingState.gravity * Time.deltaTime);
			player.playerAnimator.SetBool("Grounded", false);
		}
	}

	public override void Exit()
	{
		base.Exit();
		player.velocity = Vector2.zero;
		player.IsInCutscene = false;
	}
}
