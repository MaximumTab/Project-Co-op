using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponComp : Weapon
{
    private Weapon ParentWeapon;
    public int Index;
    public Dictionary<Collider, List<EntityManager>> OnceOnHit;
    public Dictionary<Collider, int> WeaponColliderIndex;
    public Animator Anim;
    protected int colliderIndex = 0;
    [SerializeField] private bool KillAfterDuration=true;
    public bool HoldToKeep = false;
    private float Speed;
    public override void Start()
    {
        base.Start();
        if (KillAfterDuration)
        {
            StartCoroutine(KillAfterUse());
        }
        
        WeaponColliders.AddRange(gameObject.GetComponentsInChildren<Collider>());
        OnceOnHit = new Dictionary<Collider, List<EntityManager>>();
        WeaponColliderIndex = new Dictionary<Collider, int>();
        foreach (Collider col in WeaponColliders)
        {
            OnceOnHit.Add(col,new List<EntityManager>());
            WeaponColliderIndex.Add(col,colliderIndex++);
        }

        Anim = gameObject.GetComponent<Animator>();
        if (Anim)
        {
            Anim.SetFloat("Speed", Speed);
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
        Speed = PS.SM.CurAspd();
    }

    IEnumerator KillAfterUse()
    {
        //Debug.Log("Deleting after "+WD.AbilityStruct[CompNum].AbilityDuration);
        yield return new WaitForSeconds(WD.AbilityStruct[CompNum].AbilityDuration);
        if (HoldToKeep)
        {
            yield return new WaitWhile(ParentWeapon.holdAttack);
        }

        ParentWeapon.CompScripts.Remove(Index);
        Destroy(gameObject);
        //Debug.Log("Removed");
    }
    public void CheckNoProjs()
    {
        if (WeaponColliders.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
