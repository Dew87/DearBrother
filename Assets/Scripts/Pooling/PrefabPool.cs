using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool<T> where T : PoolableBehaviour
{
	public int capacity => list.Capacity;

	private List<T> list;
	private T prefab;

	public PrefabPool(T prefab, int initialCapacity = 4)
	{
		this.prefab = prefab;
		list = new List<T>(initialCapacity);
		for (int i = 0; i < initialCapacity; i++)
		{
			CreateNewInstance();
		}
	}

	public T Spawn()
	{
		T newObject = null;
		foreach (T obj in list)
		{
			if (!obj.IsAlive())
			{
				newObject = obj;
				break;
			}
		}
		if (newObject == null)
		{
			newObject = CreateNewInstance();
		}
		newObject.Spawn();

		return newObject;
	}

	private T CreateNewInstance()
	{
		T instance = Object.Instantiate(prefab);
		list.Add(instance);
		instance.gameObject.SetActive(false);
		return instance;
	}
}
