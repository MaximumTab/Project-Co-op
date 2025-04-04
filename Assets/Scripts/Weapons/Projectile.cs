using System;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : Weapon
{

    public float Speed=10;
    private Rigidbody rb;
    public WeaponComp WC;

    public override void Start()
    {
        base.Start();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward*Speed,ForceMode.Impulse);
        CompScripts.Add(0,WC);
    }

    void Update()
    {
        gameObject.transform.LookAt(gameObject.transform.position+rb.linearVelocity);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!other.gameObject.transform.IsChildOf(PS.gameObject.transform.parent) &&!other.gameObject.transform.GetComponentInParent<Weapon>())
        {
            CompScripts[0].OnceOnHit.Remove(gameObject.GetComponentInChildren<Collider>());
            Destroy(gameObject);
            
            Debug.Log("Eaten by, "+other.gameObject.name);
        }
    }
}
