using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform CameraRotation;
    private Rigidbody rb;

    private Quaternion CameraRot;
    private Vector3 inputVector;

    [SerializeField] private float Acceleration = 20;
    [SerializeField] private float Speed=10;
    
    private bool isGrounded;
    private bool jumpCooldown=true;
    private int Jumps = 0;
    [SerializeField] private float JumpForce=10;
    [SerializeField] private int BonusJumps = 0;
    [SerializeField] private float CayoteLength=0.5f;
    [SerializeField] private float JumpWait = 0.2f;

    [SerializeField] private float DropGrav = 0.2f;
    [SerializeField] private float VertSpeed = 20f;

    private bool Firing;
    
    private Quaternion LastLook;
    private bool LookCooldown=true;
    [SerializeField] private float TurnDuration = 0.25f;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Jumps = BonusJumps;
        Physics.gravity *=1+DropGrav;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Shoot();
        Look();
        Drop();
    }

    

    void Jump()
    {
        if (isJumpable())
        {
            rb.AddForce(0,JumpForce,0,ForceMode.Impulse);
        }
    }

    bool isJumpable()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            StartCoroutine(JumpCooldown());
            return true;
        }else if (Input.GetButtonDown("Jump") && Jumps > 0&&jumpCooldown)
        {
            Jumps--;
            if (rb.linearVelocity.y<0)
            {
                rb.linearVelocity.Set(rb.linearVelocity.x,0,rb.linearVelocity.z);
            }
            StartCoroutine(JumpCooldown());
            return true;
        }
        else
        {
            return false;
        }
    }

    void Drop()
    {
        if (rb.linearVelocity.y < -VertSpeed)
        {
            rb.AddForce(-Physics.gravity);
        }
    }

    void Move()
    {
        CameraRot=Quaternion.Euler(0,CameraRotation.rotation.eulerAngles.y,0);
        inputVector =CameraRot*new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        
        rb.AddForce(SpeedLimit()*Acceleration);
    }

    Vector3 SpeedLimit()
    {
        Vector3 AdjustedSpeed = new Vector3();
        if ((rb.linearVelocity - new Vector3(0, rb.linearVelocity.y, 0)).magnitude >= Speed)
        {
            if (inputVector.x * rb.linearVelocity.x < 0)
            {
                AdjustedSpeed.x = inputVector.x;
            }

            if (inputVector.z * rb.linearVelocity.z < 0)
            {
                AdjustedSpeed.z = inputVector.z;
            }
        }
        else
        {
            AdjustedSpeed = inputVector;
        }

        return AdjustedSpeed;
    }

    void Shoot()
    {
        if (Input.GetButton("Fire1"))
        {
            Firing = true;
        }
        else
        {
            Firing = false;
        }
    }

    void Look()
    {
        if (Firing&&LookCooldown)
        {
            StartCoroutine(LerpRotation(transform.rotation, CameraRot));
        }
        else if(inputVector!=Vector3.zero&&LookCooldown)
        {
            StartCoroutine(LerpRotation(transform.rotation, Quaternion.LookRotation(inputVector)));
        }
        else
        {
            transform.rotation = LastLook;
        }
    }


    IEnumerator LerpRotation(Quaternion target, Quaternion goal)
    {
        LookCooldown = false;
        Quaternion StartRot = target;
        for(float time=0;time<TurnDuration;time+=Time.deltaTime){
            transform.rotation=Quaternion.Lerp(StartRot, goal,time/TurnDuration);
            yield return null;
        }

        LastLook = goal;
        transform.rotation = goal;
        LookCooldown = true;
        yield return null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (jumpCooldown)
        {
            isGrounded = true;
            Jumps = BonusJumps;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(CayoteTime());
    }

    IEnumerator CayoteTime()
    {
        yield return new WaitForSeconds(CayoteLength);
        isGrounded = false;
    }

    IEnumerator JumpCooldown()
    {
        jumpCooldown = false;
        isGrounded = false;
        yield return new WaitForSeconds(JumpWait);
        jumpCooldown = true;
    }
}
