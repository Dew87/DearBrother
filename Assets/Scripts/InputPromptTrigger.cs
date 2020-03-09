using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPromptTrigger : MonoBehaviour
{
	public bool hide = false;
	public Sprite gamepadSprite;
	public Sprite keyboardSprite;
	public string text;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (hide)
			{
				InputPrompt.Hide();
			}
			else
			{
				InputPrompt.Show(text, gamepadSprite, keyboardSprite);
			}
			gameObject.SetActive(false);
		}
	}
}
