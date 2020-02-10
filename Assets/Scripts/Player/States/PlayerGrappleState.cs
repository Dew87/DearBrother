using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGrappleState : PlayerState
{
    public float swingSpeed = 10f;
    public float mass = 10f;
    
    public bool isSwingingRight = true;

    private Vector2 gravity = new Vector2(0, -1);
    private float grappleLength;
    private Vector2 grappleDirection;
    private float tensionForce;
    private Vector2 pendulumSideDirection;
    private Vector2 tangentDirection;
    public override void Enter()
    {
        base.Enter();
        player.velocity.y = 0;
        grappleLength = Vector2.Distance(player.transform.position, player.grappleDetection.grapplePoint.transform.position);
        isSwingingRight = player.grappleDetection.grapplePoint.transform.position.x >= player.transform.position.x ? true : false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.CheckBoxcast(Vector2.up) && player.velocity.y > 0)
        {
            player.velocity = Vector2.zero;
        }

        player.velocity += gravity.normalized * (gravity.magnitude * mass) * Time.deltaTime;

        Vector2 playerPos = player.transform.position;
        Vector2 grapplePointPos = player.grappleDetection.grapplePoint.transform.position;

        float distanceAfterGravity = Vector2.Distance(grapplePointPos, playerPos + (player.velocity * Time.deltaTime));

        if (distanceAfterGravity > grappleLength || Mathf.Approximately(distanceAfterGravity, grappleLength))
        {
            grappleDirection = (grapplePointPos - playerPos).normalized;

            float angle = Vector2.Angle(playerPos - grapplePointPos, gravity);

            tensionForce = mass * gravity.magnitude * Mathf.Cos(Mathf.Deg2Rad * angle);
            tensionForce += ((mass * Mathf.Pow(player.velocity.magnitude, 2)) / grappleLength);

            player.velocity += grappleDirection * tensionForce * Time.deltaTime;

            player.velocity += player.velocity.normalized * (player.velocity.x > 0 ? player.horizontalInputAxis : -player.horizontalInputAxis) * swingSpeed * Time.deltaTime;
        }
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (!player.isGrappleButtonHeld)
        {
            player.TransitionState(player.fallingState);
            return;
        }
        if (player.isJumpInputPressedBuffered)
        {
            player.TransitionState(player.jumpingState);
            return;
        }
    }
}
