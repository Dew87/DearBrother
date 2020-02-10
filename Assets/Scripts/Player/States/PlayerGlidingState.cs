using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGlidingState : PlayerState
{
    public float descendSpeed = 2;
    public float verticalDeceleration = 20f;
    public float horizontalAcceleration = 20f;
    public float horizontalDeceleration = 10f;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.MoveHorizontally(player.walkingState.speed, horizontalAcceleration, horizontalDeceleration);

        float delta = player.velocity.y > -descendSpeed ? player.fallingState.gravity : verticalDeceleration;
        player.velocity.y = Mathf.MoveTowards(player.velocity.y, -descendSpeed, delta * Time.deltaTime);

        Collider2D ground = player.CheckBoxcast(Vector2.down);

        if (ground)
        {
            if (ground.TryGetComponent<Bouncer>(out Bouncer bouncer))
            {
                bouncer.Bounce(player);
                return;
            }

            if (player.velocity.x == 0)
            {
                player.TransitionState(player.standingState);
            }
            else
            {
                player.TransitionState(player.walkingState);
            }
            return;
        }
    }

    public override void Update()
    {
        base.Update();

        if (!player.isJumpInputHeld)
        {
            player.TransitionState(player.fallingState);
        }


        if (player.isGrappleButtonHeld && player.grappleDetection.grapplePoint != null)
        {
            player.TransitionState(player.grappleState);
        }
    }
}
