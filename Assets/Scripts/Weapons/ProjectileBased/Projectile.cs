using System;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : Weapon
{

    public float Speed=10;
    private Rigidbody rb;

    public override void Start()
    {
        base.Start();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward*Speed,ForceMode.Impulse);
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
            Destroy(gameObject);
            Debug.Log("Eaten by, "+other.gameObject.name);
        }
    }
}
