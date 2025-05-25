using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseEnemyManager : EntityManager
{
    public float[] OneInNumberAtkAttempt;
    protected Transform target;
    private bool targetSearched = false;
    [SerializeField] private bool stationary = true;
    [SerializeField] private float CloseEnough=10;
    
    [SerializeField] private float DistanceToActivate = 100;
    protected bool Jumpy;

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
        targetSearched = false;
        GetTarget();
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

    public override void OnDeath()
    {
        base.OnDeath();
        SoundManager.Play3DSound(SoundType.VICTORYTHEME, transform, 1f, 2f, 10f);
    }

    public override (bool,int) AtkInput() //Choose how to Shoot in Child
    {
        if (AtkDelay)
        {
            return (false,-1);
        }
        StartCoroutine(DecisionDelay());
        for (int i = 0; i < OneInNumberAtkAttempt.Length; i++)
        {
            if (Random.Range(0, OneInNumberAtkAttempt[i]) < 1&&!BusyAtk.Max())
            {
                return (true, i);
            }
        }
        return (false,-1);
    }
    public override void MoveInput()
    {
        if (!DashCool || !target||stationary) return;
        
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        if ((target.position - transform.position).magnitude<CloseEnough)
        {
            direction*=0;
        }

        MoveDir = direction;
    }


    public override void Look()
    {
        if (!target || !DashCool||stationary) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            LookDir = Quaternion.LookRotation(direction);
        }
        base.Look();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!stationary&&(target.position.y - transform.position.y)>5)
        {
            StartCoroutine(Jumping());
        }

        base.OnTriggerStay(other);
    }

    public Transform GetTarget()
    {
        if (!targetSearched || !target)
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
    public override bool JumpInput()
    {
        return Jumpy;
    }
   protected IEnumerator Jumping()
    {
        yield return new WaitForSeconds(0.1f);
        Jumpy = true;
        yield return new WaitForSeconds(0.05f);
        Jumpy = false;
    }

}

//easteregg Rickytalk