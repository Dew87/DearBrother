﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Say'", "Writes text in a dialog box. Use instead of Fungus's standard Say command.")]
[AddComponentMenu("")]
public class CustomSay : Say
{
	[SerializeField] protected bool overrideInterrupt = false;
	[SerializeField] protected bool disallowClick = false;

	public override void OnEnter()
	{
		base.OnEnter();

		if (SayDialog.ActiveSayDialog)
		{
			if (overrideInterrupt)
			{
				SayDialog.ActiveSayDialog.GetComponent<Canvas>().sortingOrder = InterruptSay.GetNewTopSortingOrder();
			}
			
			SayDialog.ActiveSayDialog.GetComponent<DearBrotherDialogInput>().ClickMode = disallowClick ? ContinueMode.Disabled : ContinueMode.Enabled;
		}

	}

	private void Reset()
	{
	}

	public void SetCharacter(Character character)
	{
		this.character = character;
	}

	public override void OnValidate()
	{
		if (character != null && character.GetComponent<DearBrotherCharacter>().type != CharacterType.Regular)
		{
			character = null;
			Debug.LogError("Can't use a *Quick or *Interrupt character with regular Say'");
		}
	}
}
