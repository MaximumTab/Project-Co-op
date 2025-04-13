using UnityEngine;

public class LawnmowerBossManager : BaseEnemyManager
{
    public override void MoveInput()
    {
        if (!DashCool || GetTarget() == null) return;

        Vector3 direction = (GetTarget().position - transform.position).normalized;
        direction.y = 0;

        MoveDir = direction;
    }


    public override void Look()
    {
        if (GetTarget() == null || !DashCool) return;

        Vector3 direction = GetTarget().position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            LookDir = Quaternion.LookRotation(direction);
        }
        base.Look();
    }

}
