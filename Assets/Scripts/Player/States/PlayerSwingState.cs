using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSwingState : PlayerGrappleBaseState
{
    public float swingSpeed = 10f;
    public float mass = 10f;

    private Vector2 gravity = new Vector2(0, -1);
    private Vector2 grappleDirection;
    private float tensionForce;
    private Vector2 pendulumSideDirection;
    private Vector2 tangentDirection;
    public override void Enter()
    {
        base.Enter();
        player.velocity.y = 0;
        player.doesDoubleJumpRemain = true;
        player.ResetGrappleInputBuffer();
    }

    public override void Update()
    {
        base.Update();
        if (player.isGrappleInputPressedBuffered)
        {
            player.grappleDetection.ReleaseGrapplePoint();
            player.TransitionState(player.fallingState);
            return;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.CheckBoxcast(Vector2.up) && player.velocity.y > 0)
        {
            player.velocity = Vector2.zero;
        }
        if ((player.CheckBoxcast(Vector2.left) || player.CheckBoxcast(Vector2.right)) && Mathf.Abs(player.velocity.x) > 0)
        {
            player.velocity = Vector2.zero;
        }
        if (player.CheckBoxcast(Vector2.down))
        {
            if (Vector2.Distance(new Vector2(player.transform.position.x + player.horizontalInputAxis * player.walkingState.speed * Time.deltaTime, player.transform.position.y), player.grappleDetection.grapplePoint.transform.position) < grappleLength)
            {
                player.MoveHorizontally(player.walkingState.speed, player.walkingState.acceleration, player.walkingState.deceleration);
            }
            else
            {
                player.velocity = Vector2.zero;
            }
        }
        else
        {
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
    }
}
