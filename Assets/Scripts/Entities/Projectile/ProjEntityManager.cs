using System;
using UnityEngine;

public class ProjEntityManager : EntityManager
{
    public bool OnAttack;
    private Projectile parProj;

    public override void Start()
    {
        base.Start();
        parProj = GetComponentInParent<Projectile>();
    }

    public override (bool, int) AtkInput()
    {
        if (OnAttack)
        {
            OnAttack = false;
            return (true, 0);
            
        }

        return (false, -1);
    }
}
