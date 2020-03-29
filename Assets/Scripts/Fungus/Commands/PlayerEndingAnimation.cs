using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Play ending animation", "Play player ending animation")]
[AddComponentMenu("")]

public class PlayerEndingAnimation : Command
{
	public override void OnEnter()
	{
		base.OnEnter();
		PlayerController.get.PlayEndAnimation();
		Continue();
	}
}
