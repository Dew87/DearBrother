using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoxCollider : MonoBehaviour
{
	public Color color = new Color(1, 0, 0.48f, 0.69f);

	private void OnDrawGizmos()
	{
		if (TryGetComponent<BoxCollider2D>(out BoxCollider2D collider))
		{
			Gizmos.color = color;
			Bounds bounds = collider.bounds;
			Gizmos.DrawCube(bounds.center, bounds.size);
		}
	}
}
