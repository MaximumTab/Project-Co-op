using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WD;
    public Collider[] WCols;

    private Animator WAnim;
    public bool[] Atking;
    private EntityManager TargetEM;
    
    public EntityManager PS;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        if (gameObject.GetComponent<Animator>())
        {
            WAnim = gameObject.GetComponent<Animator>();
        }
        Atking = new bool[WD.WNumAtks];
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
            StartCoroutine(Attacking(i));
            StartCoroutine(SpecialDuration(i));
            return true;
        }

        return false;
    }

    private bool Castable(int i)
    {
        if (WD.WAHPRFA[i].LowLim <= PS.SM.CurHpPerc() && PS.SM.CurHpPerc() <= WD.WAHPRFA[i].HighLim||WD.WAHPRFA[i].HighLim>=100&&WD.WAHPRFA[i].HighLim<=PS.SM.CurHpPerc())
        {
            return true;
        }

        return false;
    }

    void Damage(int i)
    {
        float damageAmount = PS.SM.Atk * WD.WAtkPers[i] / 100;

        TargetEM.SM.ChangeHp(-damageAmount);

        string attackerName = PS.ED != null ? PS.ED.Name : PS.gameObject.name;
        string targetName = TargetEM.ED != null ? TargetEM.ED.Name : TargetEM.gameObject.name;
        float targetCurrentHp = TargetEM.SM.Hp;

        // HUD check for Player
        if (TargetEM is PlayerManager && HealthManager.Instance[0])
        {
            HealthManager.Instance[0].TakeDamage(damageAmount);
        }

        // HUD check for Boss
        if (TargetEM is BaseEnemyManager && TargetEM.ED.isBoss&&HealthManager.Instance[1])
        {
            HealthManager.Instance[1].TakeDamage(damageAmount);
        }

        Debug.Log($"[DAMAGE] {attackerName} dealt {damageAmount} damage to {targetName}. Current HP: {targetCurrentHp}");
        if (TargetEM.SM.Hp <= 0)
        {
            PS.AddXP(TargetEM.SM.Exp);
        }
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
        for (int i = 0; i < WCols.Length; i++)
        {
            if (WCols[i] == null)
                continue;
            if(WCols[i].bounds.Intersects(other.bounds)&&WCols[i].enabled)//https://discussions.unity.com/t/is-there-a-way-to-know-which-of-the-triggers-in-a-game-object-has-triggered-the-on-trigger-enter/861484/9
            {
                Damage(i);
                break;
            }
        }
    }

    IEnumerator SpecialDuration(int i)
    {
        for (float a = 0; a < WD.WAtkDuration[i]/PS.SM.CurAspd(); a += Time.deltaTime)
        {
            SpecialAttack(i);
            yield return null;
        }
    }

    IEnumerator Attacking(int i)
    {
        Atking[i] = true;
        WAnim.SetBool("Attack"+i,true);
        yield return null;
        WAnim.SetBool("Attack"+i, false);
        WAnim.SetFloat("Speed",PS.SM.CurAspd());
        yield return new WaitForSeconds(WD.WCoolDown[i]*PS.SM.CurAcd());
        Atking[i] = false;
    }
}
 