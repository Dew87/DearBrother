using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleMotion : MonoBehaviour
{
    //Discontinued

    public GameObject grapplePoint;
    public float swingSpeed = 10f;
    public float mass = 10f;
    public Vector2 gravity = new Vector2(0, -1);
    public Vector2 velocity = new Vector3(0, 0);

    public bool isSwingingRight = true;

    private Rigidbody2D playerRB;

    private float grappleLength;
    private Vector2 grappleDirection;
    private float tensionForce;
    private Vector2 pendulumSideDirection;
    private Vector2 tangentDirection;
    private void OnDrawGizmos()
    {
        if (grapplePoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, grapplePoint.transform.position);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(velocity.x, velocity.y, 0));
        }
    }
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        if (grapplePoint != null)
        {
            grappleLength = Vector2.Distance(transform.position, grapplePoint.transform.position);
            isSwingingRight = grapplePoint.transform.position.x >= transform.position.x ? true : false;
        }
    }
    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        if (playerRB != null)
        {
            if (grapplePoint != null)
            {
                velocity += gravity.normalized * (gravity.magnitude * mass) * Time.deltaTime;

                Vector2 playerPos = transform.position;
                Vector2 grapplePointPos = grapplePoint.transform.position;

                float distanceAfterGravity = Vector2.Distance(grapplePointPos, playerPos + (velocity * Time.deltaTime));

                if (distanceAfterGravity > grappleLength || Mathf.Approximately(distanceAfterGravity, grappleLength))
                {
                    grappleDirection = (grapplePointPos - playerPos).normalized;

                    float angle = Vector2.Angle(playerPos - grapplePointPos, gravity);

                    tensionForce = mass * gravity.magnitude * Mathf.Cos(Mathf.Deg2Rad * angle);
                    tensionForce += ((mass * Mathf.Pow(velocity.magnitude, 2)) / grappleLength);

                    velocity += grappleDirection * tensionForce * Time.deltaTime;

                    velocity += velocity.normalized * (velocity.x > 0 ? move : -move) * swingSpeed * Time.deltaTime;
                }
            }
            playerRB.velocity = velocity;
        }
    }
}
