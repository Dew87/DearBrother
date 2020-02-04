using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ground movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float acceleration = 20f;
    public float deceleration = 20f;

    [Header("Air movement")]
    public float airAcceleration = 20f;
    public float airDeceleration = 0f;

    [Header("Jumping")]
    public float jumpSpeed = 20f;
    public float ascendGravity = 50f;
    public float descendGravity = 100f;
    public float jumpStopSpeed = 2f;

    private const float groundRayDistance = 0.02f;

    private Vector2 velocity;

    private Rigidbody2D rb2d;
    private new BoxCollider2D collider;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = rb2d.position.y <= -3.5f;

        // Horizontal movement //
        float move = Input.GetAxisRaw("Horizontal");

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (Mathf.Abs(velocity.x) > speed || move == 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, (grounded ? deceleration : airDeceleration) * Time.deltaTime);
        }
        else
        {
            float effectiveAccel = grounded ? acceleration : airAcceleration;
            if (Mathf.Sign(move) == -Mathf.Sign(velocity.x))
            {
                effectiveAccel += grounded ? deceleration : airDeceleration;
            }
            velocity.x = Mathf.MoveTowards(velocity.x, speed * Mathf.Sign(move), effectiveAccel * Time.deltaTime);
        }

        // Jumping //
        //var results = new RaycastHit2D[1];
        //bool grounded = rb2d.Cast(Vector2.down, results, groundRayDistance) > 0;

        if (grounded)
        {
            velocity.y = 0;
            rb2d.position = new Vector2(rb2d.position.x, -3.5f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpSpeed;
            } 
        }
        else
        {
            float gravity = velocity.y > 0 ? ascendGravity : descendGravity;
            velocity.y -= Time.deltaTime * gravity;
            if (!Input.GetKey(KeyCode.Space) && velocity.y > jumpStopSpeed)
            {
                velocity.y = jumpStopSpeed;
            }
        }

        
        rb2d.position += velocity * Time.deltaTime;

    }
}
