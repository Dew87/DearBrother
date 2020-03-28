using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private Transform soundTransform;
	public void Start()
	{
		soundTransform = GetComponent<Transform>();
		if (soundTransform == null)
		{
			Debug.LogError("Cannot find transform", this);
		}
	}
	public void SamRunSound()
	{
		FMOD.Studio.EventInstance run = FMODUnity.RuntimeManager.CreateInstance("event:/Movement/Sam/SamRun");
		run.start();
		run.release();
	}
	public void SamCrawlSound()
	{
		FMOD.Studio.EventInstance crawl = FMODUnity.RuntimeManager.CreateInstance("event:/Movement/Sam/SamCrawl");
		crawl.start();
		crawl.release();
	}
	public void SamLandSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamLand", soundTransform.position);
	}
	public void SamDieSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamDie", soundTransform.position);
	}
	public void SamGrappleSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamGrappleHook", soundTransform.position);
	}
	public void SamJumpSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamJump", soundTransform.position);
	}
	public void SamDoubleJumpSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamDoubleJump", soundTransform.position);
	}
	public void SamHappySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamHappy", soundTransform.position);
	}
	public void SamNeutralSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamNeutral", soundTransform.position);
	}
	public void SamSadSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamSad", soundTransform.position);
	}
	public void SamScaredSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamScared", soundTransform.position);
	}
	public void SamAngrySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamAngry", soundTransform.position);
	}
	public void MuteSound()
	{
		FMODUnity.RuntimeManager.MuteAllEvents(true);
	}

	public void UnmuteSound()
	{
		FMODUnity.RuntimeManager.MuteAllEvents(false);
	}
}
