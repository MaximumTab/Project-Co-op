using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CatBoss : BaseEnemyManager
{
    [SerializeField] private float[] AttackRange;

    public override (bool, int) AtkInput()
    {
        
        float targetDistance = (transform.position - target.position).magnitude;

        if (AtkDelay||targetDistance>AttackRange.Max())
        {
            return (false,-1);
        }
        StartCoroutine(DecisionDelay());
        for (int i = 0; i < OneInNumberAtkAttempt.Length; i++)
        {
            if (Random.Range(0, OneInNumberAtkAttempt[i]) < 1&&AttackRange[i]>targetDistance&&!BusyAtk.Max())
            {
                if (i == 1&&Wp.CanAtk(i))
                {
                    StartCoroutine(Jumping());
                }

                if (i == 2 && Wp.CanAtk(i))
                {
                    StartCoroutine(MakeInvis(i));
                }

                return (true, i);
            }
        }

        return (false,-1);
    }


}
