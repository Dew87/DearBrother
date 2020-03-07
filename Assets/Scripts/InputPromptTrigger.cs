using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPromptTrigger : MonoBehaviour
{
	public bool hide = false;
	public Sprite sprite;
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
				InputPrompt.Show(text, sprite); 
			}
			gameObject.SetActive(false);
		}
	}
}
