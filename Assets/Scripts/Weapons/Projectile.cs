using System.Collections;
using UnityEngine;

public class Projectile : Weapon
{

    public float Speed=10;
    protected Rigidbody rb;
    public WeaponComp WC;
    [SerializeField] private float LifeSpan=5;

    [SerializeField]private ProjEntityManager BM;

    public override void Start()
    {
        base.Start();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward*Speed,ForceMode.Impulse);
        CompScripts.Add(0,WC);
        StartCoroutine(LifeSpanExpiring());
    }

    public virtual void Update()
    {
        gameObject.transform.LookAt(gameObject.transform.position+rb.linearVelocity);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!other.gameObject.transform.IsChildOf(PS.gameObject.transform.parent) &&!other.gameObject.transform.GetComponentInParent<Weapon>()&&other.gameObject.transform.GetComponent<ProjEntityManager>()!=BM)
        {
            hitAttack(other.gameObject.name);
        }
    }

    public void hitAttack(string cause)
    {
        if (BM.Wp)
        {
            BM.Wp.WD = WD;
            BM.Wp.PS = PS;
        }

        BM.OnAttack = true;
        rb.constraints=RigidbodyConstraints.FreezePosition;
        int i = 0;
        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
                
            CompScripts[0].WeaponColliders.Remove(col);
            CompScripts[0].OnceOnHit.Remove(col);
        }
            
        if(CompScripts[0])
        {
            CompScripts[0].CheckNoProjs();
        }
            
        StartCoroutine(BM.AfterTimeRemove(BM.timeToDie));
        Debug.Log("Removed by "+cause);
        rb.linearVelocity=Vector3.zero;
        enabled = false;
    }

    IEnumerator LifeSpanExpiring()
    {
        yield return new WaitForSeconds(LifeSpan);
        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
            CompScripts[0].WeaponColliders.Remove(col);
            CompScripts[0].OnceOnHit.Remove(col);
        }
        hitAttack("Life Expiry");
    }
}
