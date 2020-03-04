using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
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
		bool isMenuInputHeld = Input.GetAxisRaw("Menu") > 0f;
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
		Time.timeScale = 0f;
		foreach (SubMenu menu in GetComponentsInChildren<SubMenu>())
		{
			menu.rootMenu.SetActive(true);
			menu.gameObject.SetActive(false);
		}
		EventSystem.current.SetSelectedGameObject(SelectedButton);
	}

	public void TriggerOff()
	{
		isMenuOn = false;
		canvas.enabled = false;
		EventSystem.current.SetSelectedGameObject(null);
		Time.timeScale = 1f;
	}
}
