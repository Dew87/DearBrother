using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Interrupt Say", "Writes text in a dialog box with an interrupt effect")]
[AddComponentMenu("")]
public class InterruptSay : Say
{
	private static int topSortingOrder = 1;

	public static int GetNewTopSortingOrder()
	{
		return ++topSortingOrder;
	}

	public static void ResetSortingOrder()
	{
		topSortingOrder = 1;
	}

	public override void OnEnter()
	{
		SayDialog dialog = Instantiate(character.SetSayDialog);
		dialog.gameObject.SetActive(true);
		setSayDialog = dialog;

		InterruptSayDialog interruptDialog = dialog.GetComponent<InterruptSayDialog>();
		interruptDialog.isTemplate = false;
		interruptDialog.Appear();

		base.OnEnter();
	}

	private void Reset()
	{
		waitForClick = false;
		fadeWhenDone = false;
	}
}
