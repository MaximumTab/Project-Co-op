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
    }


    public override void Look()
    {
        if (GetTarget() == null || isDashing) return;

        Vector3 direction = GetTarget().position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            LookDir = Quaternion.LookRotation(direction);
        }
        base.Look();
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
