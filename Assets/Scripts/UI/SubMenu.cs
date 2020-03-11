using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubMenu : MonoBehaviour
{
	public float openDuration = 0.5f;
	public float closeDuration = 0.5f;
	public float changeSelectDuration = 0.1f;

	[Space()]
	public SubMenu rootMenu;
	public PauseMenu pauseMenu;
	public GameObject defaultSelectedButton;
	public RectTransform selectIndicator;

	public event System.Action onOpen = delegate { };

	protected CanvasGroup canvasGroup;

	protected GameObject selectedButtonWhenReturning = null;
	private GameObject selectedButton;
	private GameObject previousSelectedButton;
	private float selectIndicatorMove;

	private bool isOpening;

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

	public virtual Coroutine Open(GameObject selectedButton = null)
	{
		isOpening = true;
		gameObject.SetActive(true);
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		selectedButtonWhenReturning = EventSystem.current.currentSelectedGameObject;
		previousSelectedButton = null;
		EventSystem.current.SetSelectedGameObject(selectedButton != null ? selectedButton : defaultSelectedButton);

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
			isOpening = false;
			onOpen();
		}

	}

	public virtual Coroutine Close()
	{
		if (!gameObject.activeInHierarchy)
		{
			return null;
		}

		return StartCoroutine(Transition());

		IEnumerator Transition()
		{
			while (isOpening)
			{
				yield return null;
			}

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
				rootMenu.Open(selectedButtonWhenReturning);
			}
		}
	}

	public void SetSelected(GameObject newSelectedButton)
	{
		selectIndicatorMove = 0;
		if (selectedButton != null)
		{
			previousSelectedButton = selectedButton;
		}
		selectedButton = newSelectedButton;

		if (previousSelectedButton == null && selectedButton != null)
		{
			selectIndicator.position = selectedButton.transform.position;
		}
	}

	protected virtual void Update()
	{
		if (!MemoryController.isOpen && Input.GetButtonDown("Cancel"))
		{
			Close();
		}

		if (selectIndicatorMove <= 1 && previousSelectedButton != null && selectedButton != null)
		{
			selectIndicatorMove += Time.unscaledDeltaTime / changeSelectDuration;
			selectIndicator.position = Util.VectorSmoothstep(previousSelectedButton.transform.position, selectedButton.transform.position, selectIndicatorMove);
		}
	}
}
