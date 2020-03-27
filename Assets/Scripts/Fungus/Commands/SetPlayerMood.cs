using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Set Player Mood", "")]
[AddComponentMenu("")]
public class SetPlayerMood : Command
{
	public PlayerMood mood;

	public override void OnEnter()
	{
		base.OnEnter();
		PlayerController.get.SetMood(mood);
	}

	public override string GetSummary()
	{
		return System.Enum.GetName(typeof(PlayerMood), mood);
	}
}
