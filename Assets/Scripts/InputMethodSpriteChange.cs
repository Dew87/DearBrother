using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMethodSpriteChange : MonoBehaviour
{
	public Sprite gamepadSprite;
	public Sprite keyboardSprite;

	private InputMethod lastInputMethod;

	void Start()
	{
		UpdateSprites();
	}

	private void Update()
	{
		if (lastInputMethod != InputManager.CurrentMethod)
		{
			UpdateSprites();
		}

		lastInputMethod = InputManager.CurrentMethod;
	}

	public void UpdateSprites()
	{
		if (TryGetComponent(out SpriteRenderer spriteRenderer))
		{
			spriteRenderer.sprite = GetSprite();
		}

		if (TryGetComponent(out Image image))
		{
			image.sprite = GetSprite();
		}
	}

	public Sprite GetSprite()
	{
		switch (InputManager.CurrentMethod)
		{
			case InputMethod.Gamepad: return gamepadSprite;
			case InputMethod.Keyboard: return keyboardSprite;
			default: return null;
		}
	}
}
