using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarManSoundManager : MonoBehaviour
{
	public void TarManAttackSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/TarMan/TarManAttack");
	}
	public void TarManWalkSound()
	{
		FMOD.Studio.EventInstance walk = FMODUnity.RuntimeManager.CreateInstance("event:/Movement/TarMan/TarManWalk");
		walk.start();
		walk.release();
	}
	public void TarManSadSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManSad", GetComponent<Transform>().position);
	}
	public void TarManHappySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManHappy", GetComponent<Transform>().position);
	}
	public void TarManAngrySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManAngry", GetComponent<Transform>().position);
	}
	public void TarManScaredSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManScared", GetComponent<Transform>().position);
	}
	public void TarManNeutralSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManNeutral", GetComponent<Transform>().position);
	}
	public void TarManGruntSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Misc/TarManGrunt", GetComponent<Transform>().position);
	}
}
