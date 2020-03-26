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
}
