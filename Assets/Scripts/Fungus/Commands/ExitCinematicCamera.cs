using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Exit Cinematic Camera", "Return camera to follow player")]
[AddComponentMenu("")]
public class ExitCinematicCamera : Command
{
	public bool waitUntilFinished = true;
	[Space()]
	public float duration = 1;

	public override void OnEnter()
	{
		base.OnEnter();
		PlayerCamera.get.StopCinematic(duration);
		if (waitUntilFinished)
		{
			StartCoroutine(WaitAndContinue(duration));
		}
		else
		{
			Continue();
		}
	}

	public override string GetSummary()
	{
		return "in " + duration + "s";
	}

	private IEnumerator WaitAndContinue(float duration)
	{
		yield return new WaitForSeconds(duration + 0.1f);
		Continue();
	}
}
