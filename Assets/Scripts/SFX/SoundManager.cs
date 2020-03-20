using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public AudioClip hurt, grapple, jump, land, run;
	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayOneShot(AudioClip clip)
	{
		audioSource.PlayOneShot(clip);
	}

	public void PlayRepeat(AudioClip clip)
	{
		audioSource.clip = clip;
		audioSource.Play();
	}

	public void StopSound()
	{
		audioSource.Pause();
		audioSource.clip = null;
	}

	public void MuteSound()
	{
		audioSource.volume = 0;
	}

	public void UnmuteSound()
	{
		audioSource.volume = 1;
	}
}
