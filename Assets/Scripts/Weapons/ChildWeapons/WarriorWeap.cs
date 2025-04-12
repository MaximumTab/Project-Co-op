using System.Collections;
using UnityEngine;

public class WarriorWeap : Weapon
{
    private bool LifeSteal;
    [SerializeField] private float LifeStealPercent;

    public override void Damaging(float DamageDealt)
    {
        base.Damaging(DamageDealt);
        if (LifeSteal)
        {
            PS.SM.ChangeHp(DamageDealt * WD.AbilityStruct[2].AbilityPercentages[0] / 100);
        }
    }

    public override IEnumerator SpecialDuration(int i)
    {
        switch(i)
        {
            case 2:
                LifeSteal = true;
                break;
        }

        yield return base.SpecialDuration(i);

        switch(i)
        {
            case 2:
                LifeSteal = false;
                break;
        }
    }
}
