using UnityEngine;

public class OgreEnemy : Enemy
{
    private Vector2 previousPosition;

    protected override void Start()
    {
        base.Start();

        moveSpeed = 0.75f;
        health = 5;
        expValue = 3;

        previousPosition = transform.position;
    }

    protected override void FixedUpdate()
    {
        if (player == null) return;

        previousPosition = rb.position;

        base.FixedUpdate();

        if (animator != null)
        {
            float speed = ((Vector2)transform.position - previousPosition).magnitude / Time.fixedDeltaTime;
            animator.SetFloat("Speed", speed);
        }
    }

}