using Fungus;
using System.Collections;
using UnityEngine;

[CommandInfo("*DearBrother", "Move Player", "")]
[AddComponentMenu("")]
public class MovePlayer : Command
{
	public Transform targetPosition;
	public bool glide = false;
	[Tooltip("If false, uses the player's normal walking speed instead of the value below.")]
	public bool overrideSpeed = false;
	public float speed = 10;
	[Tooltip("If true, stops player immediately when reaching target position. If false, starts decelerating when reaching target instead.")]
	public bool stopInstantly = false;
	[Tooltip("If true, doesn't execute next Fungus command until the player has reached their target")]
	public bool waitUntilFinished = true;

	public override void OnEnter()
	{
		base.OnEnter();

		if (overrideSpeed)
		{
			PlayerController.get.MoveInCutscene(targetPosition.position, speed, stopInstantly, glide);
		}
		else
		{
			PlayerController.get.MoveInCutscene(targetPosition.position, stopInstantly, glide); 
		}

		if (waitUntilFinished)
		{
			StartCoroutine(WaitUntilDone());
		}
		else
		{
			Continue();
		}
	}

	private IEnumerator WaitUntilDone()
	{
		while (PlayerController.get.currentState == PlayerController.get.cutsceneWalkingState)
		{
			yield return null;
		}

		Continue();
	}

	public override string GetSummary()
	{
		string txt = "";
		if (targetPosition != null)
		{
			txt += "--> " + targetPosition.name; 
		}
		if (overrideSpeed)
		{
			txt += " at speed " + speed;
		}
		if (stopInstantly)
		{
			txt += " (stopping instantly)";
		}
		if (glide)
		{
			txt += " (glide)";
		}
		if (!waitUntilFinished)
		{
			txt += " (don't wait)";
		}

		return txt;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Vector3 origin = transform.position;
		if (!glide)
		{
			origin.y = targetPosition.position.y;
		}
		GizmosExt.DrawArrow(origin, targetPosition.position - origin);
	}
}
