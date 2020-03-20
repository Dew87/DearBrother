using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerIntroState : PlayerState
{
	public float riseUpDuration = 0.2f;
	public float riseUpAnimationSpeed = 0.5f;

	public override void Enter()
	{
		base.Enter();
		player.playerAnimator.SetBool("Grounded", true);
		player.playerAnimator.SetBool("Moving", false);
		player.playerAnimator.Play("Sam-Undying");
		player.playerAnimator.speed = 0;
		player.IsInCutscene = true;
		player.FindCorrectGroundDistance();
	}

	public override void Exit()
	{
		base.Exit();
		player.IsInCutscene = false;
	}

	public void RiseUp()
	{
		if (player.currentState != this) return;

		player.playerAnimator.speed = riseUpAnimationSpeed;

		player.StartCoroutine(Wait());
		IEnumerator Wait()
		{
			yield return new WaitForSeconds(riseUpDuration);
			player.playerAnimator.speed = 1;
			player.TransitionState(player.standingState);
		}
	}
}
