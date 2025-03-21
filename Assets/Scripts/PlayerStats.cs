using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{
    [SerializeField,Range(1,100)] private int PCurLvl = 1;
    private int PMaxLvl = 100;
    [SerializeField] private float PBaseMaxExp = 100;
    [SerializeField] private float PMultExp = 5;
    private float PCurExp = 0;
    [SerializeField] private float PMaxExp;
    [SerializeField] private float PMaxHp;
    private float PCurHP;
    [SerializeField] public float PCurAtk {  get; private set; }=0;
    
    
    private GameObject Player;
    private class PLvl
    {
        
        public float PBaseHp {  get; private set; } = 100;
        public float PAddHp {  get; private set; } = 20;
        public float PBaseAtk { get; private set; } = 20;
        public float PAddAtk {  get; private set; } = 2;

        public PLvl()
        {
            
        }
        public PLvl(float PBH,float PAH,float PAA,float PBA)
        {
            PBaseHp = PBH;
            PAddHp = PAH;
            PBaseAtk = PBA;
            PAddAtk = PAA;
        }
    }

    private PLvl myPLvl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = gameObject.GetComponentInChildren<CharacterMovement>().gameObject;
        myPLvl = new PLvl();

    }

    private void Update()
    {
        LvlStats();
    }

    void LvlStats()//To be fully implemented further
    {
        PMaxExp = PBaseMaxExp * PMultExp * (PCurLvl-1);
        PMaxHp = myPLvl.PBaseHp + myPLvl.PAddHp * (PCurLvl-1);
        PCurAtk = myPLvl.PBaseAtk + myPLvl.PAddAtk * (PCurLvl-1);
    }
}
