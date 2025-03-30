using UnityEngine;

public class BaseEnemyManager : EntityManager
{
    public float[] InAmountPerAttack;

    public override void Start()
    {
        base.Start();

        if (ED&& ED.isBoss&& HealthManager.Instance[1])
        { 
            HealthManager.Instance[1].Init(SM.MaxHp, ED.Name);
        }
    }


    public override (bool,int) AtkInput() //Choose how to Shoot in Child
    {
        for (int i = 0; i < InAmountPerAttack.Length; i++)
        {
            if (Random.Range(0, InAmountPerAttack[i]) < 1)
            {
                return (true, i);
            }
        }
        return (false,0);
    }
    
}
