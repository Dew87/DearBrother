using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
	private Canvas canvas;

	private bool isMenuOn;

	private void Start()
	{
		canvas = GetComponent<Canvas>();
		TriggerOff();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isMenuOn)
			{
				TriggerOff();
			}
			else
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
	}

	private void TriggerOn()
	{
		isMenuOn = true;
		canvas.enabled = true;
		Time.timeScale = 0f;
	}

	private void TriggerOff()
	{
		isMenuOn = false;
		canvas.enabled = false;
		Time.timeScale = 1f;
	}
}
