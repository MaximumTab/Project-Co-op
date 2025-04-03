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
    protected int CompNum;
    public Collider[] WeaponColliders;
    public Dictionary<int,WeaponComp> CompScripts;

    private Animator WAnim;
    private bool[] Atking;
    private EntityManager TargetEM;
    
    public EntityManager PS;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        if (gameObject.GetComponent<Animator>())
        {
            WAnim = gameObject.GetComponent<Animator>();
        }
        Atking = new bool[WD.AbilityStruct.Length];
        CompScripts = new Dictionary<int, WeaponComp>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public bool Attack(int i)
    {
        if (!Atking[i]&&Castable(i))
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
        if (WD.AbilityStruct[i].LowHpLim <= PS.SM.CurHpPerc() && PS.SM.CurHpPerc() <= WD.AbilityStruct[i].HighHpLim||WD.AbilityStruct[i].HighHpLim>=100&&WD.AbilityStruct[i].HighHpLim<=PS.SM.CurHpPerc())
        {
            return true;
        }

        return false;
    }

    void Damage(int i, int CompNum)
    {
        float damageAmount = PS.SM.Atk * WD.AbilityStruct[CompNum].AbilityPercentages[i] / 100;

        if (TargetEM.SM.Hp > 0&&TargetEM.SM.Hp-damageAmount<=0)
        {
            PS.AddXP(TargetEM.SM.Exp);
        }
        TargetEM.SM.ChangeHp(-damageAmount);

        string attackerName = PS.ED != null ? PS.ED.Name : PS.gameObject.name;
        string targetName = TargetEM.ED != null ? TargetEM.ED.Name : TargetEM.gameObject.name;
        float targetCurrentHp = TargetEM.SM.Hp;

        Debug.Log($"[DAMAGE] {attackerName} dealt {damageAmount} damage to {targetName}. Current HP: {targetCurrentHp}");
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.IsChildOf(PS.gameObject.transform.parent))
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
            for (int i = 0; i < WC.WeaponColliders.Length; i++)
            {
                if (WC.WeaponColliders[i] == null)
                    continue;
                if (WC.WeaponColliders[i].bounds.Intersects(other.bounds) && WC.WeaponColliders[i].enabled) //https://discussions.unity.com/t/is-there-a-way-to-know-which-of-the-triggers-in-a-game-object-has-triggered-the-on-trigger-enter/861484/9
                {
                    Damage(i,WC.CompNum);
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

    IEnumerator SpecialDuration(int i)
    {
        for (float a = 0; a < WD.AbilityStruct[i].AbilityDuration/PS.SM.CurAspd(); a += Time.deltaTime)
        {
            SpecialAttack(i);
            yield return null;
        }
    }

    IEnumerator Attacking(int i)
    {
        Atking[i] = true;
        //WAnim.SetBool("Attack"+i,true);
        yield return null;
        //WAnim.SetBool("Attack"+i, false);
        //WAnim.SetFloat("Speed",PS.SM.CurAspd());
        yield return new WaitForSeconds(WD.AbilityStruct[i].AbilityCooldown*PS.SM.CurAcd());
        Atking[i] = false;
    }
}
 