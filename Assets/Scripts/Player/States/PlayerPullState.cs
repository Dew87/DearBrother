using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPullState : PlayerGrappleBaseState
{
    public float speed = 5;
    public float acceleration = 20;
    public float deceleration = 20;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.MoveHorizontally(speed, acceleration, deceleration);
    }
    public override void Update()
    {
        base.Update();

        if (!player.CheckOverlaps(Vector2.down))
        {
            player.TransitionState(player.fallingState);
            return;
        }
    }
}
