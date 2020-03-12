using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarManSoundManager : MonoBehaviour
{
	public AudioClip attack, walk;

	private AudioSource audioSource;

	private void Awake()
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
}
