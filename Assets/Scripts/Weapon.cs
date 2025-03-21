using System;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WD;
    public Collider[] WCols;
    
    [SerializeField] private PlayerStats PS;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DamageMelee(int i)
    {
        Debug.Log(PS.PCurAtk*WD.WAtkPers[i]+" Was done as damage");
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < WCols.Length; i++)
        {
            if(WCols[i].bounds.Intersects(other.bounds))//https://discussions.unity.com/t/is-there-a-way-to-know-which-of-the-triggers-in-a-game-object-has-triggered-the-on-trigger-enter/861484/9
            {
                DamageMelee(i);
                return;
            }
        }
    }
}
 