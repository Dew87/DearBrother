using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputMethod
{
	Keyboard,
	Gamepad
}

public class InputManager : MonoBehaviour
{
	private const float axisThreshold = 0.2f;

	private static InputManager instance;
	private static InputManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<InputManager>();

				if (instance == null)
				{
					Debug.LogError("There needs to be one active InputManager script on a GameObject in your scene.");
				}
			}
			return instance;
		}
	}

	public static InputMethod CurrentMethod => Instance.currentMethod;

	[SerializeField] private InputMethod currentMethod;
	//[SerializeField] private float anyAxis;

	private KeyCode[] joystickButtons = new KeyCode[] {
		KeyCode.JoystickButton0,
		KeyCode.JoystickButton1,
		KeyCode.JoystickButton2,
		KeyCode.JoystickButton3,
		KeyCode.JoystickButton4,
		KeyCode.JoystickButton5,
		KeyCode.JoystickButton6,
		KeyCode.JoystickButton7,
		KeyCode.JoystickButton8,
		KeyCode.JoystickButton9,
	};

	// Update is called once per frame
	void Update()
	{
		if (Input.anyKeyDown)
		{
			currentMethod = InputMethod.Keyboard;
		}

		float anyAxis = Input.GetAxisRaw("AnyControllerAxis");
		if (Mathf.Abs(anyAxis) > axisThreshold)
		{
			currentMethod = InputMethod.Gamepad;
		}
		else
		{
			foreach (var key in joystickButtons)
			{
				if (Input.GetKeyDown(key))
				{
					currentMethod = InputMethod.Gamepad;
				}
			}
		}
	}
}
