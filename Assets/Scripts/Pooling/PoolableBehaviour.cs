using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableBehaviour : MonoBehaviour
{
	public virtual void Spawn()
	{
		gameObject.SetActive(true);
	}

	public virtual bool IsAlive()
	{
		return gameObject.activeSelf;
	}

	public virtual void Die()
	{
		gameObject.SetActive(false);
	}
}