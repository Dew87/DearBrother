using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Say'", "Writes text in a dialog box. Use instead of Fungus's standard Say command.")]
[AddComponentMenu("")]
public class CustomSay : Say
{
	[SerializeField] protected bool overrideInterrupt = true;
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
			
			SayDialog.ActiveSayDialog.GetComponent<DialogInput>().ClickMode = disallowClick ? ClickMode.Disabled : ClickMode.ClickAnywhere;
		}

	}

	private void Reset()
	{
	}
}
