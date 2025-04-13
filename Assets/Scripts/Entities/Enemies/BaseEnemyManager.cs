using UnityEngine;

public class BaseEnemyManager : EntityManager
{
    public float[] OneInNumberAtkAttempt;
    private Transform target;
    private bool targetSearched = false;

    public override void Start()
    {
        base.Start();

        if (ED&& ED.isBoss&& HealthManager.Instance[1])
        { 
            HealthManager.Instance[1].Init(SM.MaxHp, ED.Name);
        }
        Anim = gameObject.GetComponentInParent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        if (ED&& ED.isBoss&& HealthManager.Instance[1])
        { 
            HealthManager.Instance[1].SetCurHp(SM.Hp);
        }
        
    }

    public override (bool,int) AtkInput() //Choose how to Shoot in Child
    {
        for (int i = 0; i < OneInNumberAtkAttempt.Length; i++)
        {
            if (Random.Range(0, OneInNumberAtkAttempt[i]) < 1)
            {
                return (true, i);
            }
        }
        return (false,0);
    }

    public Transform GetTarget()
    {
        if (!targetSearched || target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj)
            {
                target = playerObj.transform;
            }
            targetSearched = true;
        }

        return target;
    }
}

//easteregg Rickytalk