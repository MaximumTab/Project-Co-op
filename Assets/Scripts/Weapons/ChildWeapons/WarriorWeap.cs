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
            PS.SM.ChangeHp(DamageDealt * LifeStealPercent / 100);
        }
    }

    public override IEnumerator SpecialDuration(int i)
    {
        if (i == 2)
        {
            LifeSteal = true;
        }

        yield return base.SpecialDuration(i);

        if (i == 2)
        {
            LifeSteal = false;
        }
    }
}
