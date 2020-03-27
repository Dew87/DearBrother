using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODMusicManager : MonoBehaviour
{
	FMOD.Studio.EventInstance credits;
	FMOD.Studio.EventInstance chase;
	FMOD.Studio.EventInstance easterEgg;
	FMOD.Studio.EventInstance mainMenu;
	FMOD.Studio.EventInstance worldTheme;
	FMOD.Studio.EventInstance worldTheme2;
	private IEnumerator StartGameParameter()
	{
		float param = 0;
		while (param < 1)
		{
			if (param + Time.deltaTime < 1)
			{
				param += Time.deltaTime;
			}
			else
			{
				param = 1;
			}
			mainMenu.setParameterByName("Start Game", 1);
			yield return null;
		}
	}
	public void Start()
	{
		credits = FMODUnity.RuntimeManager.CreateInstance("event:/Credits");
		chase = FMODUnity.RuntimeManager.CreateInstance("event:/Chase");
		easterEgg = FMODUnity.RuntimeManager.CreateInstance("event:/Easter Egg");
		mainMenu = FMODUnity.RuntimeManager.CreateInstance("event:/Main Menu");
		worldTheme = FMODUnity.RuntimeManager.CreateInstance("event:/World Theme");
		worldTheme2 = FMODUnity.RuntimeManager.CreateInstance("event:/World Theme 2");
	}
	public void CreditsMusic()
	{
		chase.stop(0);
		easterEgg.stop(0);
		mainMenu.stop(0);
		worldTheme.stop(0);
		worldTheme2.stop(0);
		credits.start();
	}
	public void ChaseMusic()
	{
		credits.stop(0);
		easterEgg.stop(0);
		mainMenu.stop(0);
		worldTheme.stop(0);
		worldTheme2.stop(0);
		chase.start();
	}
	public void EasterEggMusic()
	{
		credits.stop(0);
		chase.stop(0);
		mainMenu.stop(0);
		worldTheme.stop(0);
		worldTheme2.stop(0);
		easterEgg.start();
	}
	public void MainMenuMusic()
	{
		credits.stop(0);
		chase.stop(0);
		easterEgg.stop(0);
		worldTheme.stop(0);
		worldTheme2.stop(0);
		mainMenu.start();
	}
	public void EndMainMenuMusic()
	{
		StartCoroutine(StartGameParameter());
	}
	public void WorldThemeMusic()
	{
		credits.stop(0);
		chase.stop(0);
		easterEgg.stop(0);
		mainMenu.stop(0);
		worldTheme2.stop(0);
		worldTheme.start();
	}
	public void WorldTheme2Music()
	{
		credits.stop(0);
		chase.stop(0);
		easterEgg.stop(0);
		mainMenu.stop(0);
		worldTheme.stop(0);
		worldTheme2.start();
	}
	public void StopMusic()
	{
		credits.stop(0);
		chase.stop(0);
		easterEgg.stop(0);
		mainMenu.stop(0);
		worldTheme.stop(0);
		worldTheme2.stop(0);
	}
	public void PauseOn()
	{
		chase.setParameterByName("Pause Menu", 1);
		worldTheme.setParameterByName("Pause Menu", 1);
		worldTheme2.setParameterByName("Pause Menu", 1);
	}
	public void PauseOff()
	{
		chase.setParameterByName("Pause Menu", 0);
		worldTheme.setParameterByName("Pause Menu", 0);
		worldTheme2.setParameterByName("Pause Menu", 0);
	}
}
