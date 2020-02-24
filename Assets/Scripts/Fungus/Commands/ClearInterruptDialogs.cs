using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Clear Interrupt Dialogs", "Hide all currently active Interrupt Dialogs")]
[AddComponentMenu("")]
public class ClearInterruptDialogs : Command
{
	public override void OnEnter()
	{
		base.OnEnter();

		foreach (InterruptSayDialog dialog in FindObjectsOfType<InterruptSayDialog>())
		{
			if (!dialog.isTemplate)
			{
				Destroy(dialog.gameObject); 
			}
		}

		foreach (SayDialog dialog in FindObjectsOfType<SayDialog>())
		{
			dialog.GetComponent<Canvas>().sortingOrder = 1;
		}

		InterruptSay.ResetSortingOrder();

		Continue();
	}
}
