using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TarManWalkingState : TarManState
{
    public float speed = 2f;
    public Vector2 patrolPosition1;
    public Vector2 patrolPosition2;
    public bool walkToPosition2First = false;

    private Vector2 targetPosition;

    public override void Start()
    {
        base.Start();

        targetPosition = walkToPosition2First ? patrolPosition2 : patrolPosition1;
    }

    public override void OnValidate()
	{
		base.OnValidate();

        Collider2D collider = tarMan.GetComponent<Collider2D>();
        float y;
        if (collider)
        {
            y = collider.bounds.min.y;
        }
        else
        {
            y = tarMan.transform.position.y;
        }
        patrolPosition1 = new Vector2(patrolPosition1.x, y);
        patrolPosition2 = new Vector2(patrolPosition2.x, y);
    }

    public override void Reset()
    {
        base.Reset();

        patrolPosition1 = tarMan.transform.position + Vector3.left * 2f;
        patrolPosition2 = tarMan.transform.position + Vector3.right * 2f;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(patrolPosition1, patrolPosition2);
        DrawLineEnd(patrolPosition1);
        DrawLineEnd(patrolPosition2);

        void DrawLineEnd(Vector2 position)
        {
            const float size = 0.5f;
            Gizmos.DrawLine(new Vector2(position.x, position.y - size * 0.5f), new Vector2(position.x, position.y + size * 0.5f));
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector2 position = tarMan.rb2d.position;
        position.x = Mathf.MoveTowards(position.x, targetPosition.x, speed * Time.deltaTime);
        tarMan.rb2d.position = position;
        if (position.x == targetPosition.x)
        {
            targetPosition = targetPosition.x == patrolPosition1.x ? patrolPosition2 : patrolPosition1;
        }
    }
}
