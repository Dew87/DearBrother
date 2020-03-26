using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
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
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamLand", GetComponent<Transform>().position);
	}
	public void SamDieSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamDie", GetComponent<Transform>().position);
	}
	public void SamGrappleSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamGrappleHook", GetComponent<Transform>().position);
	}
	public void SamJumpSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamJump", GetComponent<Transform>().position);
	}
	public void SamDoubleJumpSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Sam/SamDoubleJump", GetComponent<Transform>().position);
	}
	public void SamHappySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamHappy", GetComponent<Transform>().position);
	}
	public void SamNeutralSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamNeutral", GetComponent<Transform>().position);
	}
	public void SamSadSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamSad", GetComponent<Transform>().position);
	}
	public void SamScaredSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamScared", GetComponent<Transform>().position);
	}
	public void SamAngrySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sam/SamAngry", GetComponent<Transform>().position);
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
