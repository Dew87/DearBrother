using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Scenario", "Set up scenario (put first in Flowcart")]
[AddComponentMenu("")]
public class Scenario : Command
{
	[Tooltip("If true, this scenario will only happen once, not every time the player enters the trigger")]
	public bool onlyTriggerOnce = true;
	[Tooltip("If true, the scenario will be reset (can be triggered again) after the player dies")]
	public bool resetOnDeath = true;

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
		Debug.Log("start listen");
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
		Debug.Log("stop listen");
	}

	public override void OnEnter()
	{
		base.OnEnter();

		if (onlyTriggerOnce)
		{
			Debug.Log("Disable");
			GetComponent<Collider2D>().enabled = false;
		}

		Continue();
	}

	private void OnPlayerDeath()
	{
		if (resetOnDeath)
		{
			Debug.Log("Reenable");
			GetComponent<Collider2D>().enabled = true;
		}
	}

	public override string GetSummary()
	{
		string txt = "";
		if (onlyTriggerOnce) txt += " (Only Once) ";
		if (resetOnDeath) txt += " (Reset On Death) ";
		return txt;
	}
}
