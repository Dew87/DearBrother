using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryButton : MonoBehaviour
{
	public Collectible collectible;

	private Image image;
	private Animator animator;
	private Button button;

	private void Awake()
	{
		image = GetComponent<Image>();
		animator = GetComponent<Animator>();
		button = GetComponent<Button>();
	}

	public void View()
	{
		collectible.ShowMemory();
	}

	public void Refresh()
	{
		if (collectible.isCollected)
		{
			animator.speed = 0;
			animator.Play(collectible.animationName);
			button.interactable = true;
		}
		else
		{
			animator.Play("CollectibleEmpty");
			button.interactable = false;
		}
	}
}
