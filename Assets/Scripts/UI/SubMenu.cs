using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubMenu : MonoBehaviour
{
	public float openDuration = 0.5f;
	public float closeDuration = 0.5f;

	[Space()]
	public SubMenu rootMenu;
	public PauseMenu pauseMenu;
	public GameObject selectedButton;

	public event System.Action onOpen = delegate { };

	protected CanvasGroup canvasGroup;

	protected GameObject selectedButtonWhenReturning = null;

	protected virtual void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void OpenMenu()
	{
		Open();
	}

	public void CloseMenu()
	{
		Close();
	}

	public virtual Coroutine Open()
	{
		gameObject.SetActive(true);
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		selectedButtonWhenReturning = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(selectedButton);
		return StartCoroutine(Transition());

		IEnumerator Transition()
		{
			if (rootMenu)
			{
				yield return rootMenu.Close();
			}

			float t = 0;
			while (t <= 1)
			{
				t += Time.unscaledDeltaTime / openDuration;
				canvasGroup.alpha = t;
				yield return null;
			}

			canvasGroup.interactable = true;
			pauseMenu.currentMenu = this;
			onOpen();
		}

	}

	public virtual Coroutine Close()
	{
		return StartCoroutine(Transition());

		IEnumerator Transition()
		{
			float t = 0;
			while (t <= 1)
			{
				t += Time.unscaledDeltaTime / closeDuration;
				canvasGroup.alpha = 1 - t;
				yield return null;
			}

			EventSystem.current.SetSelectedGameObject(null);
			gameObject.SetActive(false);
			if (rootMenu)
			{
				rootMenu.Open();
			}
			EventSystem.current.SetSelectedGameObject(selectedButtonWhenReturning);
		}

	}

	protected virtual void Update()
	{
		if (!MemoryController.isOpen && Input.GetButtonDown("Cancel"))
		{
			Close();
		}
	}
}
