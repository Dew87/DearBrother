using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlSFX : MonoBehaviour
{
	private Transform owlTransform;
	public void Start()
	{
		owlTransform = GetComponent<Transform>();
		if (owlTransform == null)
		{
			Debug.LogError("Cannot find owl transform");
		}
	}
	public void OwlSadSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlSad", owlTransform.position);
	}
	public void OwlHappySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlHappy", owlTransform.position);
	}
	public void OwlAngrySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlAngry", owlTransform.position);
	}
	public void OwlScaredSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlScared", owlTransform.position);
	}
	public void OwlNeutralSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Owl/OwlNeutral", owlTransform.position);
	}
	public void OwlWingSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Movement/Owl/OwlWing", owlTransform.position);
	}
	public void OwlDeathSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Misc/OwlDeath", owlTransform.position);
	}
}
