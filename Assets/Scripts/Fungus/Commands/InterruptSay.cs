using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Interrupt Say", "Writes text in a dialog box with an interrupt effect")]
[AddComponentMenu("")]
public class InterruptSay : Say
{
	public override void OnEnter()
	{
		SayDialog dialog = Instantiate(character.SetSayDialog);
		dialog.GetComponent<InterruptSayDialog>().isTemplate = false;
		setSayDialog = dialog;
		Canvas dialogCanvas = dialog.GetComponent<Canvas>();
		dialogCanvas.sortingOrder = 10;

		base.OnEnter();
	}

	private void Reset()
	{
		waitForClick = false;
		fadeWhenDone = false;
	}
}
