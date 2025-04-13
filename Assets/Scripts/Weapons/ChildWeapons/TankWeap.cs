using UnityEngine;
using System.Collections;
public class TankWeap : Weapon
{
    public override IEnumerator SpecialDuration(int i)
    {
        switch(i)
        {
            case 2:
                PS.DmgReductPercBuff(WD.AbilityStruct[i].AbilityPercentages[0], WD.AbilityStruct[i].AbilityDuration/PS.SM.CurAspd());
                break;
            case 1:
                StartCoroutine(PS.Dashing(PS.rb.transform.forward));
                break;
        }

        yield return base.SpecialDuration(i);

    }
}
