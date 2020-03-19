using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarManAttackDetection : MonoBehaviour
{
	private TarManController tarMan;

	private void Start()
	{
		tarMan = GetComponentInParent<TarManController>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		IKillable killable = collision.GetComponentInParent<IKillable>();
		if (killable != null)
		{
			Vector2 position = tarMan.transform.position;
			Vector2 target = collision.transform.position;
			Vector2 direction = target - position;

			tarMan.FaceDirection(direction);
			tarMan.TransitionState(tarMan.attackState);
		}
	}
}
