using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCrawlingState : PlayerState
{
    public float speed = 2;
    public float acceleration = 20;
    public float deceleration = 20;

    public override void Enter()
    {
        base.Enter();
        player.SetCollider(player.crouchingCollider);
        player.spriteRenderer.transform.localScale = new Vector3(1, 0.5f);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetCollider(player.normalCollider);
        player.spriteRenderer.transform.localScale = Vector3.one;
    }

    public override void Update()
    {
        base.Update();

        player.ResetJumpGraceTimer();

        if (!player.isCrouchInputHeld)
        {
            player.TransitionState(player.walkingState);
            return;
        }

        if (player.velocity.x == 0)
        {
            player.TransitionState(player.crouchingState);
            return;
        }

        if (player.isJumpInputPressedBuffered)
        {
            player.TransitionState(player.jumpingState);
            return;
        }

        if (!player.CheckBoxcast(Vector2.down))
        {
            player.TransitionState(player.fallingState);
            return;
        }

        if (player.isGrappleButtonHeld && player.grappleDetection.grapplePoint != null)
        {
            player.TransitionState(player.grappleState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.MoveHorizontally(speed, acceleration, deceleration);
    }
}