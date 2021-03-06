﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
	public SubMenu defaultMenu;
	public Graphic darkness;
	public float darknessFadeInDuration = 0.5f;
	public FMODMusicManager cameraFMODMusicManager;

	[HideInInspector] public SubMenu currentMenu;

	private bool isInSubMenu => currentMenu != defaultMenu;
	private Canvas canvas;
	private bool isMenuOn;
	private float darknessAlpha;

	private void Start()
	{
		canvas = GetComponent<Canvas>();
		canvas.enabled = false;
		isMenuOn = false;
		darknessAlpha = darkness.color.a;
	}

	private void Update()
	{
		if (!MemoryController.isOpen && !PlayerController.get.isFrozen && PlayerController.get.currentState != PlayerController.get.introState)
		{
			if (isMenuOn)
			{
				bool isCancelPressed = Input.GetButtonDown("Cancel");
				if ((Input.GetButtonDown("Menu") && !(isCancelPressed && isInSubMenu)) || (isCancelPressed && !isInSubMenu))
				{
					TriggerOff();
				}
			}
			else
			{
				if (Input.GetButtonDown("Menu"))
				{
					TriggerOn();
				}
			}
		}
	}

	public void Quit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void RestartCheckpoint()
	{
		//EventManager.TriggerEvent("PlayerDeath");
		PlayerController.get.GetComponent<PlayerHealth>().TakeDamage();
		TriggerOff();
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		cameraFMODMusicManager.StopMusic();
		Time.timeScale = 1;
	}

	private void TriggerOn()
	{
		isMenuOn = true;
		canvas.enabled = true;
		Time.timeScale = 0f;
		foreach (SubMenu menu in GetComponentsInChildren<SubMenu>())
		{
			menu.gameObject.SetActive(false);
		}
		defaultMenu.Open();
		cameraFMODMusicManager.PauseOn();
		StartCoroutine(Transition());
		IEnumerator Transition()
		{
			Color color = darkness.color;
			float t = 0;
			while (t <= 1)
			{
				color.a = Mathf.Lerp(0, darknessAlpha, t);
				darkness.color = color;
				t += Time.unscaledDeltaTime / darknessFadeInDuration;
				yield return null;
			}
		}
	}

	public void TriggerOff()
	{
		defaultMenu.Close();
		cameraFMODMusicManager.PauseOff();
		StartCoroutine(Transition());
		IEnumerator Transition()
		{
			Color color = darkness.color;

			float t = 0;
			while (t <= 1)
			{
				color.a = Mathf.Lerp(darknessAlpha, 0, t);
				darkness.color = color;
				t += Time.unscaledDeltaTime / darknessFadeInDuration;
				yield return null;
			}

			isMenuOn = false;
			canvas.enabled = false;
			EventSystem.current.SetSelectedGameObject(null);
			Time.timeScale = 1f;
		}
	}
}
