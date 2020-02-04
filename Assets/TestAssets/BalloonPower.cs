using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPower : MonoBehaviour
{
    private Rigidbody2D rb;
    public float fallMultiplier = 0.5f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale *= fallMultiplier;
    }
}
