using UnityEngine;

public class BeakWeap : Weapon
{
    public float MultSpeed=1;
    public override void SpecialAttack(int i)
    {
        switch(i)
        {
            case 2:
                PS.rb.linearVelocity = PS.rb.rotation * Vector3.forward * (PS.ED.Speed * MultSpeed)+Vector3.up*PS.rb.linearVelocity.y;
                break;
        }
    }
}
