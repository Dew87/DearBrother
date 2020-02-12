using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Range(0, 360f)]
    [Tooltip("Degrees, 0 = right, 90 = up, etc.")]
    public float direction = 270f;
    public float speed = 3f;

    private float timer;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
