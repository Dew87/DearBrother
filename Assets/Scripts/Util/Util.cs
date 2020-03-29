using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
	public static Vector3 VectorSmoothstep(Vector3 from, Vector3 to, float t)
	{
		return new Vector3(Mathf.SmoothStep(from.x, to.x, t), Mathf.SmoothStep(from.y, to.y, t), Mathf.SmoothStep(from.z, to.z, t));
	}

	public static Vector3 Polar(float length, float angle)
	{
		return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * length;
	}

	// http://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
	public static float DeltaTimedDamp(float a, float b, float damping, float dt)
	{
		return Mathf.Lerp(a, b, 1 - Mathf.Pow(1 - damping, dt));
	}
}
