using UnityEngine;

public class SkeletonEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();

        moveSpeed = 1f;
        health = 1;
        expValue = 1;
    }

}