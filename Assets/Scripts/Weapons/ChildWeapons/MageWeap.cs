using System.Collections;
using UnityEngine;

public class MageWeap : Weapon
{
    public override IEnumerator SpecialDuration(int i)
    {
        switch(i)
        {
            case 2:
                PS.AspdPercBuff(WD.AbilityStruct[2].AbilityPercentages[0], WD.AbilityStruct[i].AbilityDuration/PS.SM.CurAspd());
                break;
        }

        yield return base.SpecialDuration(i);

    }
}
