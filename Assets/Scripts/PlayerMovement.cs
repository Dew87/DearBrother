using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float acceleration = 20f;
    public float deceleration = 20f;

    private Rigidbody2D rb2d;
    private Vector2 velocity;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (Mathf.Abs(velocity.x) > speed || move == 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }
        else
        {
            float effectiveAccel = acceleration;
            if (Mathf.Sign(move) == -Mathf.Sign(velocity.x))
            {
                effectiveAccel += deceleration;
            }
            velocity.x = Mathf.MoveTowards(velocity.x, speed * Mathf.Sign(move), effectiveAccel * Time.deltaTime);
        }

        
        rb2d.position += velocity * Time.deltaTime;

    }
}
