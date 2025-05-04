using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyManager : EntityManager
{
    public float[] OneInNumberAtkAttempt;
    private Transform target;
    private bool targetSearched = false;
    [SerializeField] private float DistanceToActivate = 100;

    public override void Start()
    {
        base.Start();
        if (ED && ED.isBoss)
        { 
            HealthManager.RegisterBoss(this, SM.MaxHp, ED.Name);
            if (HealthManager.Instance[1])
            {
                HealthManager.Instance[1].Init(SM.MaxHp, ED.Name);
            }
        }
        Anim = gameObject.GetComponentInParent<Animator>();
    }

    public override void Update()
    {
        float distanceToPlayer = (transform.position - GetTarget().position).magnitude;

        if (distanceToPlayer < DistanceToActivate)
        {
            base.Update();
            if (ED && ED.isBoss)
            {
                if (!HealthManager.IsBossRegistered(this))
                {
                    HealthManager.RegisterBoss(this, SM.MaxHp, ED.Name);
                }


                if (SM.Hp <= 0)
                {
                    HealthManager.UnregisterBoss(this);
                }
            }
        }
        else
        {
            if (ED && ED.isBoss)
            {
                if (HealthManager.IsBossRegistered(this))
                {
                    HealthManager.UnregisterBoss(this);
                }
            }
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
        return (false,-1);
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