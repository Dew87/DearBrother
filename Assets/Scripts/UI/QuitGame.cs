using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitGame : MonoBehaviour
{
	public Text quitText;

	private float timer = 0;
	private float timeToQuit = 66;
	private float timeToEscape = 5;
	private void Update()
    {
		if (timer < timeToQuit)
		{
			timer += Time.deltaTime;
		}
		if (timer > timeToEscape)
		{
			if (InputManager.CurrentMethod == InputMethod.Keyboard)
			{
				quitText.text = "Press Escape To Quit";
			}
			else
			{
				quitText.text = "Press Start To Quit";
			}
		}
		if (timer > timeToQuit || (timer > timeToEscape && Input.GetButtonDown("Menu")))
		{
			#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
    }
}
