﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEjector : MonoBehaviour
{
	public float interval = 0.2f;
	[Tooltip("Timer start value (0-interval), to make different ejectors that are not in sync")]
	public float cycleOffset = 0f;
	public Vector2 spawnPosition;
	public Projectile projectilePrefab;
	[Tooltip("Amount of projectiles to spawn immediately (at correct distance from each other)")]
	public int prewarmAmount = 6;

	[Space()]
	[Tooltip("Degrees, 0 = right, 90 = up, etc")]
	[Range(0,360f)]
	[UnityEngine.Serialization.FormerlySerializedAs("directionOverride")]
	public float fireDirection = 180f;

	private PrefabPool<Projectile> pool;

	private float timer;

	private void Awake()
	{
		timer = Mathf.Repeat(cycleOffset, interval);
		pool = new PrefabPool<Projectile>(projectilePrefab);
		Prewarm();
	}

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnPlayerDeath()
	{
		timer = Mathf.Repeat(cycleOffset, interval);
		Prewarm();
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= interval)
		{
			timer = Mathf.Repeat(timer, interval);
			SpawnProjectile();
		}
	}

	private void Prewarm()
	{
		for (int i = 0; i < prewarmAmount; i++)
		{
			float time = interval * i + cycleOffset;
			Projectile newProjectile = SpawnProjectile();
			if (!newProjectile.FastForward(time))
			{
				break;
			}
		}
	}

	private Projectile SpawnProjectile()
	{
		Projectile newProjectile = pool.Spawn();
		newProjectile.transform.position = transform.TransformPoint(spawnPosition);
		newProjectile.direction = fireDirection;
		return newProjectile;
	}
}
