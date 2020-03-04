using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TarManIdleState : TarManState
{
	public override void Enter()
	{
		base.Enter();

		tarMan.rb2d.velocity = Vector2.zero;
	}
}
