using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseEnemyManager : EntityManager
{
    public float[] OneInNumberAtkAttempt;
    private Transform target;
    private bool targetSearched = false;
    [SerializeField] private float DistanceToActivate=100;

    private List<Renderer> flashRenderers = new List<Renderer>();
    private List<Color> originalColors = new List<Color>();
    private Coroutine flashCoroutine;

    public override void Start()
    {
        base.Start();
        Renderer[] foundRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in transform.root.GetComponentsInChildren<Renderer>(true))
        {
            if (r.gameObject.name.ToLower().Contains("weapon")) continue; 
            flashRenderers.Add(r);
            r.material = new Material(r.material);
            originalColors.Add(r.material.color);
        }
        if (ED&& ED.isBoss&& HealthManager.Instance[1])
        { 
            HealthManager.Instance[1].Init(SM.MaxHp, ED.Name);
        }
        Anim = gameObject.GetComponentInParent<Animator>();
    }

    public override void Update()
    {
        if ((transform.position - GetTarget().position).magnitude < DistanceToActivate)
        {
            base.Update();
        }

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

    public void OnDamaged()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRed());
    }
    private IEnumerator FlashRed()
    {
        for (int i = 0; i < flashRenderers.Count; i++)
        {
            flashRenderers[i].material.color = new Color(0.8f, 0.3f, 0.3f); 
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < flashRenderers.Count; i++)
        {
            flashRenderers[i].material.color = originalColors[i];
        }
    }

}

//easteregg Rickytalk