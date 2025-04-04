using System.Collections;
using UnityEngine;

public class MageWeap : Weapon
{
    [SerializeField] private float AspdBuffDuration;
    public override IEnumerator SpecialDuration(int i)
    {
        if (i == 2)
        {
            PS.AspdPercBuff(WD.AbilityStruct[2].AbilityPercentages[0], AspdBuffDuration);
        }

        yield return base.SpecialDuration(i);

    }
}
