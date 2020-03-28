using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarManSoundManager : MonoBehaviour
{
	private Transform tarmanTransform;
	private void Start()
	{
		tarmanTransform = GetComponent<Transform>();
		if (tarmanTransform == null)
		{
			Debug.LogError("Cannot find tarMan Transform", this);
		}
	}
	public void TarManAttackSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/TarMan/TarManAttack", tarmanTransform.position);
	}
	public void TarManWalkSound()
	{
		FMOD.Studio.EventInstance walk = FMODUnity.RuntimeManager.CreateInstance("event:/Movement/TarMan/TarManWalk");
		walk.start();
		walk.release();
	}
	public void TarManSadSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManSad", tarmanTransform.position);
	}
	public void TarManHappySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManHappy", tarmanTransform.position);
	}
	public void TarManAngrySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManAngry", tarmanTransform.position);
	}
	public void TarManScaredSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManScared", tarmanTransform.position);
	}
	public void TarManNeutralSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/TarMan/TarManNeutral", tarmanTransform.position);
	}
	public void TarManGruntSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Misc/TarManGrunt", tarmanTransform.position);
	}
}
