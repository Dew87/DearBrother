using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
	public float inputThreshold = 0.1f;

	public GameObject SelectedButton;

	private Canvas canvas;

	private bool isMenuOn;
	private bool MenuInputIsTriggered;

	private void Start()
	{
		canvas = GetComponent<Canvas>();
		canvas.enabled = false;
		isMenuOn = false;
		MenuInputIsTriggered = false;
	}

	private void Update()
	{
		bool isMenuInputHeld = Input.GetAxisRaw("Menu") > inputThreshold;
		if (isMenuInputHeld && !MenuInputIsTriggered)
		{
			MenuInputIsTriggered = true;
			if (isMenuOn)
			{
				TriggerOff();
			}
			else
			{
				TriggerOn();
			}
		}
		if (!isMenuInputHeld)
		{
			MenuInputIsTriggered = false;
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
		EventManager.TriggerEvent("PlayerDeath");
		TriggerOff();
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		TriggerOff();
	}

	private void TriggerOn()
	{
		isMenuOn = true;
		canvas.enabled = true;
		EventSystem.current.SetSelectedGameObject(SelectedButton);
		Time.timeScale = 0f;
	}

	private void TriggerOff()
	{
		isMenuOn = false;
		canvas.enabled = false;
		SelectedButton = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(null);
		Time.timeScale = 1f;
	}
}
