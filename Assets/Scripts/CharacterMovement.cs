using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform CameraRotation;
    private Rigidbody rb;

    [SerializeField] private GameObject Weapon;
    private Weapon Wp;

    private Quaternion CameraRot;
    private Vector3 inputVector;
    private Vector2 moveInput;

    [SerializeField] private float Acceleration = 20;
    [SerializeField] private float Speed=10;
    
    private bool isGrounded;
    private bool jumpCooldown=true;
    private int Jumps = 0;
    [SerializeField] private float JumpForce=10;
    [SerializeField] private int BonusJumps = 0;
    [SerializeField] private float CayoteLength=0.5f;
    [SerializeField] private float JumpWait = 0.2f;

    [SerializeField] private float DashDistance = 5;
    [SerializeField] private float BaseDashDist = 20;
    [SerializeField] private float DashDownTime = 2;
    [SerializeField] private float DashDuration = 0.1f;
    private bool DashCool = true;

    [SerializeField] private float DropGrav = 0.2f;
    [SerializeField] private float TerminalVel = 20f;

    private bool Firing;
    
    private Quaternion LastLook;
    private bool LookCooldown=true;
    [SerializeField] private float TurnDuration = 0.25f;

    private InputSystem_Actions input;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Jumps = BonusJumps;
        Physics.gravity *=1+DropGrav;
        Wp = Weapon.GetComponent<Weapon>();
        input = new InputSystem_Actions();
        input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = input.Player.Move.ReadValue<Vector2>();

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
        if (input.Player.Jump.triggered && isGrounded)
        {
            StartCoroutine(JumpCooldown());
            return true;
        }else if (input.Player.Jump.triggered && Jumps > 0&&jumpCooldown)
        {
            Jumps--;
            if (rb.linearVelocity.y<0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z);
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
        if (rb.linearVelocity.y < -TerminalVel)
        {
            rb.AddForce(-Physics.gravity);
        }
    }

    void Move()
    {
        CameraRot=Quaternion.Euler(0,CameraRotation.rotation.eulerAngles.y,0);
        inputVector =CameraRot*new Vector3(moveInput.x,0, moveInput.y);
        Dash();
        rb.AddForce(SpeedLimit()*Acceleration);
    }

    void Dash()
    {
        if (isDash())
        {
            StartCoroutine(Dashing());
        }
    }

    bool isDash()
    {
        if (input.Player.Sprint.triggered && DashCool)
        {
            StartCoroutine(DashCoolDown());
            return true;
        }
        else
        {
            return false;
        }
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
        if (input.Player.Attack.IsPressed()&&!Firing)
        {
            StartCoroutine(WFiring());
        }
    }

    IEnumerator WFiring()
    {
        Firing = true;
        Wp.Attack();
        Weapon.transform.rotation = CameraRotation.rotation;
        for (float i = 0; i < Wp.WD.WCoolDown; i += Time.deltaTime)
        {
            Weapon.transform.position = gameObject.transform.position;
            yield return null;
        }
        Firing = false;
        yield return null;
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
        yield return null;
        LookCooldown = true;
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

    IEnumerator DashCoolDown()
    {
        DashCool = false;
        yield return new WaitForSeconds(DashDownTime);
        DashCool = true;
    }

    IEnumerator Dashing()
    {
        Vector3 Start = rb.linearVelocity;
        Vector3 End = inputVector * (DashDistance * BaseDashDist);
        for (float time = 0; time < DashDuration; time += Time.deltaTime)
        {
            rb.linearVelocity = Vector3.Lerp(Start, End, time / DashDuration);
            yield return null;
        }
        rb.linearVelocity = End;
        yield return null;

    }
}
