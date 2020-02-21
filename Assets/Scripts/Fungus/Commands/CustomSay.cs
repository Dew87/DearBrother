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
		if (overrideInterrupt)
		{
			InterruptSayDialog dialog = FindObjectOfType<InterruptSayDialog>();
			if (dialog)
			{
				dialog.canvas.sortingOrder = -1; 
			}
		}

		base.OnEnter();
	}

	private void Reset()
	{
	}
}
