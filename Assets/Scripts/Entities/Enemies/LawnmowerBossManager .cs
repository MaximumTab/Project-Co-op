using UnityEngine;

public class LawnmowerBossManager : BaseEnemyManager
{
    private Transform target;
    private bool targetSearched = false;
    public bool isDashing = false;


    public override void MoveInput()
    {
        if (isDashing || GetTarget() == null) return;

        Vector3 direction = (GetTarget().position - transform.position).normalized;
        direction.y = 0;

        MoveDir = direction;

        float speed = 10f;
        rb.linearVelocity = direction * speed;
    }


    public override void Look()
    {
        if (GetTarget() == null) return;

        Vector3 lookDirection = (target.position - transform.position);
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    public Transform GetTarget()
    {
        if (!targetSearched || target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj)
            {
                target = playerObj.transform;
            }
            targetSearched = true;
        }

        return target;
    }
}
