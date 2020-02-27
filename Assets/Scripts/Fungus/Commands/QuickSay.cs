using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Quick Say", "Writes non-intrusive text.")]
[AddComponentMenu("")]
public class QuickSay : Say
{
	public float duration = 1f;

	public override void OnEnter()
	{
		base.OnEnter();

		StartCoroutine(DoWait());
	}

	private IEnumerator DoWait()
	{
		yield return new WaitForSeconds(duration);
		Writer writer = character.SetSayDialog.Writer;
		while (!writer.IsWaitingForInput)
		{
			yield return null;
		}
		character.SetSayDialog.Clear();
		Continue();
	}

	public override void OnValidate()
	{
		if (character != null && character.GetComponent<DearBrotherCharacter>().type != CharacterType.Quick)
		{
			character = null;
			Debug.LogError("You must use a *Quick character with Quick Say");
		}
	}
}
