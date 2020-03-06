using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
	public GameObject SelectedButton;

	[HideInInspector] public bool isInSubMenu;

	private Canvas canvas;

	private bool isMenuOn;

	private void Start()
	{
		canvas = GetComponent<Canvas>();
		canvas.enabled = false;
		isMenuOn = false;
	}

	private void Update()
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
			menu.rootMenu.gameObject.SetActive(true);
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
