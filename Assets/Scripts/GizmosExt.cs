using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosExt
{
	public static void DrawArrow(Vector2 origin, Vector2 vector)
	{
		const float capHeight = 0.2f;
		const float capWidth = 0.1f;

		Vector2 direction = vector.normalized;
		Vector2 perpendicular = Vector2.Perpendicular(direction);
		Vector2 capBottomCenter = origin + vector - direction * capHeight;
		Vector2 capBottom1 = capBottomCenter - perpendicular * capWidth * 0.5f;
		Vector2 capBottom2 = capBottomCenter + perpendicular * capWidth * 0.5f;

		Gizmos.DrawLine(origin, origin + vector);
		Gizmos.DrawLine(capBottom1, capBottom2);
		Gizmos.DrawLine(capBottom1, origin + vector);
		Gizmos.DrawLine(capBottom2, origin + vector);
	}
}
