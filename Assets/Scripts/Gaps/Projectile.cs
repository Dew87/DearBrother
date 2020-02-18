using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolableBehaviour
{
    [Range(0, 360f)]
    [Tooltip("Degrees, 0 = right, 90 = up, etc.")]
    public float direction = 270f;
    public float speed = 3f;

    private Rigidbody2D rb2d;

    private float timer;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

	private void OnEnable()
	{
		EventManager.StartListening("PlayerDeath", OnPlayerDeath);
	}

	private void OnDisable()
	{
		EventManager.StopListening("PlayerDeath", OnPlayerDeath);
	}

	private void FixedUpdate()
    {
        float angle = direction * Mathf.Deg2Rad;
        Vector2 directionVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        rb2d.position += directionVector * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Solid"))
        {
            Die();
        }
    }

	private void OnPlayerDeath()
	{
		Die();
	}
}
