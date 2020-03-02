using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightenAnimal : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D collision)
	{
		Transform parent = collision.transform.parent;
		if (parent && parent.TryGetComponent<AnimalWhipBehaviour>(out AnimalWhipBehaviour animal))
		{
			animal.Frighten(new Vector2(Mathf.Sign(animal.transform.position.x - transform.position.x), 0));
		}
	}
}
