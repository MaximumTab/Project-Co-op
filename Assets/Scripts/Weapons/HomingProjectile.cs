using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HomingBullet : Projectile
{
    [SerializeField] float yellowBeeHomCancleDist; 
    [SerializeField] bool isYellow;
    private GameObject target;
    private Vector3 targetVelocity;
    private bool isHomingFixed;
    private bool hasHomingHappened;


    public override void Start()
    {
        ///rb = gameObject.GetComponent<Rigidbody>();
        base.Start(); 
        target = FindAnyObjectByType<PlayerManager>().gameObject;
    }

    public override void Update()
    {
        
        Vector3 direction = target.transform.position - gameObject.transform.position; //vector pointing to player
        targetVelocity = Vector3.ClampMagnitude(direction, 1) * Speed;
        
        if (isHomingFixed)
        {
            rb.linearVelocity = targetVelocity;
        }
        else if (!hasHomingHappened)
        {           
            StartCoroutine(InitialHoming());
        }

        base.Update();

        if (isYellow && direction.magnitude < yellowBeeHomCancleDist)
        {
            isHomingFixed = false;
        } 

        
    }

    IEnumerator InitialHoming()
    {
        hasHomingHappened = true;
        isHomingFixed = false;
        Vector3 Start = rb.linearVelocity;
        Vector3 End = targetVelocity;
        float flightCorrectionDuration = Random.Range(3, 5);

        for (float time = 0; time < flightCorrectionDuration; time += Time.deltaTime)
        {
            rb.linearVelocity = Vector3.Lerp(Start, End, time / flightCorrectionDuration);
            yield return null;
        }
        rb.linearVelocity = End;
        isHomingFixed = true;
        yield return null;

    }
    
}
