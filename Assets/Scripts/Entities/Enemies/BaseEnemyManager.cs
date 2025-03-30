using UnityEngine;

public class BaseEnemyManager : EntityManager
{
    public float[] InAmountPerAttack;

    public override void Start()
    {
        base.Start();

        if (ED != null && ED.Name == "Rock" && BossHealthManager.Instance != null)
        {
            BossHealthManager.Instance.Init(SM.MaxHp, ED.Name);
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
