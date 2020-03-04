using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryButton : MonoBehaviour
{
	public Collectible collectible;

	public void View()
	{
		collectible.ShowMemory();
	}
}
