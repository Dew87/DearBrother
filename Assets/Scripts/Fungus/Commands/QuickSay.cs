using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("*DearBrother", "Timed Say", "Writes unskippable text with a timer")]
[AddComponentMenu("")]
public class QuickSay : Say
{
	public float duration = 1f;
	public bool overrideTextForGamepads = false;
	[TextArea(5, 10)]
	public string gamepadText;

	private string keyboardText;

	private void Awake()
	{
		keyboardText = storyText;
	}

	public override void OnEnter()
	{
		if (overrideTextForGamepads)
		{
			storyText = InputManager.CurrentMethod == InputMethod.Gamepad ? gamepadText : keyboardText;
		}

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
		character.SetSayDialog.GetComponent<DearBrotherDialogInput>().SetNextLineFlag();
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
