using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlSFX : MonoBehaviour
{
    public void OwlSadSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlSad", GetComponent<Transform>().position);
	}
	public void OwlHappySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlHappy", GetComponent<Transform>().position);
	}
	public void OwlAngrySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlAngry", GetComponent<Transform>().position);
	}
	public void OwlScaredSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlScared", GetComponent<Transform>().position);
	}
	public void OwlNeutralSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlNeutral", GetComponent<Transform>().position);
	}
	public void OwlWingSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Owl/OwlWing", GetComponent<Transform>().position);
	}
	public void OwlDeathSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Misc/OwlDeath", GetComponent<Transform>().position);
	}
}
