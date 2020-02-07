using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLandingLagState : PlayerState
{
	public float duration = 0.3f;

	private float timer;

	public override void Enter()
	{
		base.Enter();

		timer = duration;
		player.velocity = Vector3.zero;
		player.transform.rotation = Quaternion.Euler(0, 0, 10f);
	}

	public override void Exit()
	{
		base.Exit();

		player.transform.rotation = Quaternion.identity;
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
