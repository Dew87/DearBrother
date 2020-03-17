using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Freeze Player", "Freeze or unfreeze the player")]
[AddComponentMenu("")]
public class FreezePlayer : Command
{
	public bool freezePlayer = true;
	public bool waitUntilGrounded = false;

	public override void OnEnter()
	{
		base.OnEnter();

		if (freezePlayer)
		{
			PlayerController.get.TransitionState(PlayerController.get.cutsceneStandingState);

			if (!waitUntilGrounded)
			{
				Continue();
			}
			else
			{
				StartCoroutine(WaitUntilGrounded());
			} 
		}
		else
		{
			PlayerController.get.TransitionState(PlayerController.get.standingState);

			Continue();
		}
	}

	public override string GetSummary()
	{
		return freezePlayer ? "Freeze" : "Unfreeze";
	}

	private IEnumerator WaitUntilGrounded()
	{
		while (!PlayerController.get.CheckOverlaps(Vector2.down))
		{
			yield return null;
		}
		Continue();
	}
}
