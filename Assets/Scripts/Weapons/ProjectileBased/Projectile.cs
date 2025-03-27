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
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Destroy(gameObject);
    }
}
