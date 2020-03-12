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
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	public override void OnEnter()
	{
		base.OnEnter();

		if (onlyTriggerOnce)
		{
			GetComponent<Collider2D>().enabled = false;
		}

		Continue();
	}

	private void OnPlayerDeath()
	{
		if (resetOnDeath)
		{
			GetComponent<Collider2D>().enabled = true;
		}
	}

	private void Start()
	{
		GetComponent<Collider2D>().enabled = true;
	}

	public override string GetSummary()
	{
		string txt = "";
		if (onlyTriggerOnce) txt += " (Only Once) ";
		if (resetOnDeath) txt += " (Reset On Death) ";
		return txt;
	}

	private void OnDrawGizmosSelected()
	{
		// For some unknown reason, the box collider on the Fungus trigger object is constantly disabled in the prefab.
		// So here's a hack to make sure it's reenabled.
#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			GetComponent<Collider2D>().enabled = true;
		}
#endif
	}
}
