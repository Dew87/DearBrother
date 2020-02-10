using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerFallingState : PlayerState
{
    public float maxFallSpeed = 20;
    [Tooltip("If the player has fallen for at least this manys seconds when landing, landing lag occurs")]
    public float landingLagDurationThreshold = 1f;
    public float gravity = 100;
    public float acceleration = 20;
    public float deceleration = 10;

    private float landingLagTimer;

    public override void Enter()
    {
        base.Enter();

        landingLagTimer = landingLagDurationThreshold;
    }

    public override void Exit()
    {
        base.Exit();

        player.ResetJumpGraceTimer();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.MoveHorizontally(player.walkingState.speed, acceleration, deceleration);

        player.velocity.y = Mathf.MoveTowards(player.velocity.y, -maxFallSpeed, gravity * Time.deltaTime);

        if (landingLagTimer > 0)
        {
            landingLagTimer -= Time.deltaTime;
        }

        Collider2D ground = player.CheckBoxcast(Vector2.down);

        if (ground)
        {
            if (ground.TryGetComponent<VolatilePlatform>(out VolatilePlatform platform))
            {
                if (player.velocity.y < -platform.breakSpeed)
                {
                    platform.Break(); 
                }
            }


            if (landingLagTimer > 0)
            {
                if (player.velocity.x == 0)
                {
                    player.TransitionState(player.standingState);
                }
                else
                {
                    player.TransitionState(player.walkingState);
                }
            }
            else
            {
                player.TransitionState(player.landingLagState);
            }
            return;
        }

        if (player.jumpGraceTimer > 0 && player.isJumpInputPressedBuffered)
        {
            player.TransitionState(player.jumpingState);
        }
        else if (player.hasDoubleJump && player.doesDoubleJumpRemain && player.isJumpInputPressedBuffered)
        {
            player.TransitionState(player.doubleJumpingState);
        }
        else if (player.isJumpInputHeld)
        {
            player.TransitionState(player.glidingState);
        }
        else if (player.isGrappleButtonHeld && player.grappleDetection.grapplePoint != null)
        {
            player.TransitionState(player.grappleState);
        }
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
