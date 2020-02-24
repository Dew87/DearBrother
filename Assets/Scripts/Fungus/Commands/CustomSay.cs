using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Say'", "Writes text in a dialog box. Use instead of Fungus's standard Say command.")]
[AddComponentMenu("")]
public class CustomSay : Say
{
	[SerializeField] protected bool overrideInterrupt = true;

	public override void OnEnter()
	{
		base.OnEnter();

		if (overrideInterrupt)
		{
			if (SayDialog.ActiveSayDialog)
			{
				SayDialog.ActiveSayDialog.GetComponent<Canvas>().sortingOrder = InterruptSay.GetNewTopSortingOrder(); 
			}
		}
	}

	private void Reset()
	{
	}
}
