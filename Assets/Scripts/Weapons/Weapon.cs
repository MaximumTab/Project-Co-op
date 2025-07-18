using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WD;
    public GameObject[] WeaponComponents;
    public int CompNum;
    public List<Collider> WeaponColliders=new List<Collider>();
    public bool HoldingAttack { get; private set; }
    public Dictionary<int,WeaponComp> CompScripts;

    private Animator WAnim;
    private bool[] Atking;
    private EntityManager TargetEM;
    private int prevAttack=-1;
    
    public EntityManager PS;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        if (gameObject.GetComponent<Animator>())
        {
            WAnim = gameObject.GetComponent<Animator>();
        }

        if (WD)
        {
            Atking = new bool[WD.AbilityStruct.Length];
        }

        CompScripts = new Dictionary<int, WeaponComp>();
    }

    public void RemoveMe()
    {
        Destroy(gameObject);
    }

    public virtual void SpecialAttack(int i)
    {
        /*
         switch(i)
         {
            case 0:
                break;
         }
         */
    }

    public void SetCompNum(int CNum)
    {
        CompNum = CNum;
    }

    public void ChangePrev(int i)
    {
        //Debug.Log(prevAttack+", "+i+" from "+PS.name);
        if (prevAttack == i&&i!=-1)
        {
            HoldingAttack = true;
        }
        else
        {
            HoldingAttack = false;
            prevAttack = i;
        }

        
    }

    public bool CanAtk(int i)
    {
        return !Atking[i] && Castable(i);
    }

    public bool Attack(int i)
    {

        if (CanAtk(i))
        {
            if (WeaponComponents.Length>i&& WeaponComponents[i])
            {
                GameObject WC = Instantiate(WeaponComponents[i], gameObject.transform);
                WC.GetComponent<WeaponComp>().GiveStats(i,this);
                int Index = FindEmptyKey();
                CompScripts.Add(Index,WC.GetComponent<WeaponComp>());
                WC.GetComponent<WeaponComp>().Index = Index;
            }

            StartCoroutine(Attacking(i));
            StartCoroutine(SpecialDuration(i));
            return true;
        }

        return false;
    }

    private bool Castable(int i)
    {
        if (WD.AbilityStruct[i].LowHpLim < PS.SM.CurHpPerc() && PS.SM.CurHpPerc() <= WD.AbilityStruct[i].HighHpLim||WD.AbilityStruct[i].HighHpLim>=100&&WD.AbilityStruct[i].HighHpLim<=PS.SM.CurHpPerc())
        {
            return true;
        }

        return false;
    }

    public virtual void Damaging(float DamageDealt)
    {
        TargetEM.SM.ChangeHp(-DamageDealt);
        TargetEM.OnDamaged();

    }

    void Damage(int i, int CompNum)
    {
        float damageAmount = PS.SM.Atk * WD.AbilityStruct[CompNum].AbilityPercentages[i] / 100;

        if (TargetEM.SM.Hp > 0&&TargetEM.SM.Hp-damageAmount<=0)
        {
            PS.AddXP(TargetEM.SM.Exp);
        }
        Damaging(damageAmount);

        string attackerName = PS.ED != null ? PS.ED.Name : PS.gameObject.name;
        string targetName = TargetEM.ED != null ? TargetEM.ED.Name : TargetEM.gameObject.name;
        float targetCurrentHp = TargetEM.SM.Hp;

        //Debug.Log($"[DAMAGE] {attackerName} dealt {damageAmount} damage to {targetName}. Current HP: {targetCurrentHp}");
    }

    void Knocking(int i, int CompNum)
    {
        float KnockAmount = KnockingAmount(WD.AbilityStruct[CompNum].KnockBackStrengths, i);
        float KnockUpAmount = KnockingAmount(WD.AbilityStruct[CompNum].KnockUpStrengths,i);
        Vector3 KnockDirection = ((TargetEM.transform.position - transform.position)-Vector3.up*(TargetEM.transform.position.y - transform.position.y)).normalized;
        TargetEM.rb.AddForce(KnockDirection*KnockAmount+Vector3.up*KnockUpAmount,ForceMode.Impulse);
    }

    float KnockingAmount(float[] knockAmounts, int targInt)
    {
        if (knockAmounts == null)
            return 0;
        if (knockAmounts.Length > targInt)
        {
            return knockAmounts[targInt];
        }
        if(knockAmounts.Length!=0)
        {
            return knockAmounts[knockAmounts.Length - 1];
        }

        return 0;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.IsChildOf(PS.gameObject.transform.parent) )
            return;
        TargetEM=null;
        if (other.gameObject.GetComponent<EntityManager>())
        {
            TargetEM = other.gameObject.GetComponent<EntityManager>();
        }
        else
        {
            return;
        }
        foreach (WeaponComp WC in CompScripts.Values)
        {
            foreach (Collider col in WC.OnceOnHit.Keys)
            {
                if (!col)
                {
                    continue;
                }
                if (col.bounds.Intersects(other.bounds) &&col.enabled && !WC.HitEMYet(col, TargetEM)) //https://discussions.unity.com/t/is-there-a-way-to-know-which-of-the-triggers-in-a-game-object-has-triggered-the-on-trigger-enter/861484/9
                {
                    Damage(WC.WeaponColliderIndex[col],WC.CompNum);
                    Knocking(WC.WeaponColliderIndex[col],WC.CompNum);
                    //add force to other.gameobject
                    break;
                }
            }
        }
    }
    private int FindEmptyKey()
    {
        for (int i = 0; i < CompScripts.Count; i++)
        {
            if (!CompScripts.ContainsKey(i))
            {
                return i;
            }
        }
        return CompScripts.Count;
    }

    public virtual IEnumerator SpecialDuration(int i)
    {
        for (float a = 0; a < WD.AbilityStruct[i].AbilityDuration/PS.SM.CurAspd(); a += Time.deltaTime)
        {
            SpecialAttack(i);
            yield return null;
        }
    }

    public bool holdAttack()
    {
        return HoldingAttack;
    }
    IEnumerator Attacking(int i)
    {
        Atking[i] = true;
        //WAnim.SetBool("Attack"+i,true);
        yield return null;
        //WAnim.SetBool("Attack"+i, false);
        //WAnim.SetFloat("Speed",PS.SM.CurAspd());
        yield return new WaitForSeconds(WD.AbilityStruct[i].AbilityCooldown*PS.SM.CurAcd());
        
        foreach (WeaponComp WC in CompScripts.Values)
        {
            if (WC.HoldToKeep&&WC.CompNum==i)
            {
                yield return new WaitWhile(holdAttack);
                break;
            }
        }
        Atking[i] = false;
    }
}
 