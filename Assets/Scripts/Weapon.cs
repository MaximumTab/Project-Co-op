using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WD;
    public Collider[] WCols;

    private Animator WAnim;
    private bool Atking;
    
    [SerializeField] private EntityManager PS;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WAnim = gameObject.GetComponent<Animator>();
        Atking = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(int i)
    {
        if (!Atking)
        {
            StartCoroutine(Attacking(i));
        }
    }

    void DamageMelee(int i)
    {
        //Debug.Log(PS.PCurAtk*WD.WAtkPers[i]/100+" Was done as damage");
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.transform.IsChildOf(PS.gameObject.transform))
            return;*/
        for (int i = 0; i < WCols.Length; i++)
        {
            if(WCols[i].bounds.Intersects(other.bounds)&&WCols[i].enabled)//https://discussions.unity.com/t/is-there-a-way-to-know-which-of-the-triggers-in-a-game-object-has-triggered-the-on-trigger-enter/861484/9
            {
                DamageMelee(i);
            }
        }
    }

    IEnumerator Attacking(int i)
    {
        Atking = true;
        WAnim.SetBool("Attack"+i,true);
        yield return new WaitForSeconds(WD.WCoolDown[1]);
        WAnim.SetBool("Attack"+i, false);
        Atking = false;
    }
}
 