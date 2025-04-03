using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponComp : Weapon
{
    private Weapon ParentWeapon;
    public int Index;
    private Dictionary<Collider, List<EntityManager>> OnceOnHit;
    public override void Start()
    {
        base.Start();
        StartCoroutine(KillAfterUse());
        WeaponColliders =gameObject.GetComponentsInChildren<Collider>();
        OnceOnHit = new Dictionary<Collider, List<EntityManager>>();
        foreach (Collider col in WeaponColliders)
        {
            OnceOnHit.Add(col,new List<EntityManager>());
        }
    }

    public bool HitEMYet(Collider i,EntityManager TargetEM)
    {
        if (OnceOnHit[i].Contains(TargetEM))
        {
            return true;
        }

        OnceOnHit[i].Add(TargetEM);
        return false;
    }

    public void GiveStats(int CN,Weapon inheritWeapon)
    {
        CompNum=CN;
        //Debug.Log(CN);
        WD = inheritWeapon.WD;
        PS = inheritWeapon.PS;
        ParentWeapon = inheritWeapon;
    }

    IEnumerator KillAfterUse()
    {
        //Debug.Log("Deleting after "+WD.AbilityStruct[CompNum].AbilityDuration);
        yield return new WaitForSeconds(WD.AbilityStruct[CompNum].AbilityDuration);
        Destroy(ParentWeapon.CompScripts[Index]);
        Destroy(gameObject);
    }
}
