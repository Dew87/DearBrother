using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamAnimationSFXTrigger : MonoBehaviour
{
	public SoundManager soundManager;
	public void Run()
	{
		if (soundManager != null)
		{
			soundManager.SamRunSound();
		}
	}
	public void Crawl()
	{
		if (soundManager != null)
		{
			soundManager.SamCrawlSound();
		}
	}
	public void Grapple()
	{
		if (soundManager != null)
		{
			soundManager.SamGrappleSound();
		}
	}
}
