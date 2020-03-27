using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamAnimationSFXTrigger : MonoBehaviour
{
	public SoundManager soundManager;
	public void Run()
	{
		soundManager.SamRunSound();
	}
	public void Crawl()
	{
		soundManager.SamCrawlSound();
	}
}
