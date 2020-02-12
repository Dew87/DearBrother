using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEjector : MonoBehaviour
{
	public float interval = 0.2f;
	[Tooltip("Timer start value (0-interval), to make different ejectors that are not in sync")]
	public float cycleOffset = 0f;
	public Vector2 spawnPosition;
	public Projectile projectilePrefab;
	[Space()]
	[Tooltip("If checked,  the projectile's direction is set to directionOverride, ignoring the projectile prefab's own value")]
	public bool overrideDirection = false;
	[Range(0, 360f)]
	[Tooltip("Degrees, 0 = right, 90 = up, etc. Only applies if overrideDirection is ticked")]
	public float directionOverride;

	private float timer;
	private PrefabPool<Projectile> pool;

	private void Awake()
	{
		timer = Mathf.Repeat(cycleOffset, interval);
		pool = new PrefabPool<Projectile>(projectilePrefab);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= interval)
		{
			timer = Mathf.Repeat(timer, interval);
			Projectile newProjectile = pool.Spawn();
			newProjectile.transform.position = transform.TransformPoint(spawnPosition);
			if (overrideDirection)
			{
				newProjectile.direction = directionOverride;
			}
		}
	}
}
