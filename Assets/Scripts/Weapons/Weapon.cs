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

    public void Attack(int i)
    {
        if (!Atking[i]&&Castable(i))
        {
            StartCoroutine(Attacking(i));
        }
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
        Debug.Log(PS.SM.Atk*WD.WAtkPers[i]/100+" Was done as damage");
        TargetEM.SM.ChangeHp(-PS.SM.Atk*WD.WAtkPers[i]/100);
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
 