using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLandingLagState : PlayerState
{
	public float duration = 0.1f;

	private float timer;

	public override void Enter()
	{
		base.Enter();
		player.soundManager.SamLandSound();
		player.playerAnimator.SetBool("Grounded", true);
		timer = duration;
		player.velocity = Vector3.zero;
		player.spriteRenderer.flipY = true;
	}

	public override void Exit()
	{
		base.Exit();

		player.spriteRenderer.flipY = false;
	}

	public override void Update()
	{
		base.Update();

		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			player.TransitionState(player.standingState);
		}
	}
}
