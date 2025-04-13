using System.Collections;
using UnityEngine;

public class TankGooseWeap : Weapon
{
    public override IEnumerator SpecialDuration(int i)
    {
        switch (i)
        {
            case 0 :
                yield return ShieldPush(); //slash
                break;
           //eventually put another attack here
        }
        
    }

    public IEnumerator ShieldPush()
    {
        StartCoroutine(PS.DashCoolDown());
        yield return PS.Dashing(PS.transform.forward);  
    }
    
}
