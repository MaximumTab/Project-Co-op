using UnityEngine;

public class TankGoose : BaseEnemyManager
{
   
    public override (bool, int) AtkInput()
    {
        float targetDistance = (transform.position - GetTarget().position).magnitude;
        return base.AtkInput();
    }
    
    
    public override void MoveInput()
    {
        if (!DashCool || !GetTarget()) return;

        Vector3 direction = (GetTarget().position - transform.position).normalized;
        direction.y = 0;

        MoveDir = direction;
    }


    public override void Look()
    {
        if (!GetTarget() || !DashCool) return;

        Vector3 direction = GetTarget().position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            LookDir = Quaternion.LookRotation(direction);
        }
        base.Look();
    }

}
