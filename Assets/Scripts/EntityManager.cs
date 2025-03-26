using System.Collections;
using UnityEngine;
public class EntityManager : MonoBehaviour
{
    public EntityData ED;
    private Rigidbody rb;
    public Vector3 MoveDir;
    public Quaternion LookDir;
    public Quaternion LastLook;
    public bool LookCooldown=true;
    public float TurnDuration = 0.25f;
    
    [SerializeField] private GameObject Weapon;
    private Weapon Wp;
    
    public int Lvl;
    public float Atk;
    public float Hp;
    public float MaxHp;
    public float Aspd;


    public float Acceleration;
    public float Speed;
    
    private bool isGrounded;
    private bool jumpCooldown=true;
    private int Jumps = 0;
    
    public float JumpForce=10;
    public int BonusJumps = 0;
    public float CayoteLength=0.5f;
    public float JumpWait = 0.2f;
    
    public float DashDistance = 5;
    public float BaseDashDist = 20;
    public float DashDownTime = 2;
    public float DashDuration = 0.1f;
    private bool DashCool = true;
    
    public float DropGrav = 0.2f;
    public float TerminalVel = 20f;
    
    public bool Attacking;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Jumps = BonusJumps;
        if (Weapon != null)
        {
            Wp = Weapon.GetComponent<Weapon>();
        }
        LevelUp();
        OnChildStart();
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

    public void LevelUp()
    {
        Hp = ED.BaseHp + ED.GrowHp * Lvl;
        MaxHp = ED.BaseHp + ED.GrowHp * Lvl;
        Atk = ED.BaseAtk + ED.GrowAtk * Lvl;
        Lvl++;
    }

    public void ChangeHp(float AddHp)
    {
        Hp += AddHp;
    }

    public virtual void OnChildStart()
    {
    }
    public virtual void MoveInput()//Change MoveDir in Child
    {
        
    }
    public virtual (bool,int) AtkInput() //Choose how to Shoot in Child
    {
        return (false,0);
    }
    public virtual bool JumpInput() //Choose how to Jump in Child
    {
        return false;
    }
    public virtual bool DashInput() //Choose how to Dash in Child
    {
        return false;
    }
    bool isJumpable()
    {
        if (JumpInput() && isGrounded)
        {
            StartCoroutine(JumpCooldown());
            return true;
        }else if (JumpInput() && Jumps > 0&&jumpCooldown)
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
    
    void Shoot()
    {
        if (AtkInput().Item1&&!Attacking)
        {
            StartCoroutine(WFiring(AtkInput().Item2));
        }
    }
    void Jump()
    {
        if (isJumpable())
        {
            rb.AddForce(0,JumpForce,0,ForceMode.Impulse);
        }
    }
    void Drop()
    {
        if (rb.linearVelocity.y < -TerminalVel)
        {
            rb.AddForce(-Physics.gravity);
        }
        else if(rb.linearVelocity.y<-1)
        {
            rb.AddForce(new Vector3(0,-DropGrav,0));
        }
    }
    public virtual void Look()
    {
        if (MoveDir != Vector3.zero&&LookCooldown)
        {
            StartCoroutine(LerpRotation(transform.rotation, Quaternion.LookRotation(MoveDir)));
        }
        else
        {
            transform.rotation = LastLook;
        }
    }
    void Move()
    {
        MoveInput();
        Dash();
        rb.AddForce(SpeedLimit()*Acceleration);
    }

    void Dash()
    {
        if (DashInput()&& DashCool)
        {
            StartCoroutine(Dashing());
            StartCoroutine(DashCoolDown());
        }
    }
    
    Vector3 SpeedLimit()
    {
        Vector3 AdjustedSpeed = new Vector3();
        if ((rb.linearVelocity - new Vector3(0, rb.linearVelocity.y, 0)).magnitude >= Speed)
        {
            if (MoveDir.x * rb.linearVelocity.x < 0)
            {
                AdjustedSpeed.x = MoveDir.x;
            }

            if (MoveDir.z * rb.linearVelocity.z < 0)
            {
                AdjustedSpeed.z = MoveDir.z;
            }
        }
        else
        {
            AdjustedSpeed = MoveDir;
        }

        return AdjustedSpeed;
    }
    
    public IEnumerator LerpRotation(Quaternion target, Quaternion goal)
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
        Vector3 End = MoveDir * (DashDistance * BaseDashDist);
        for (float time = 0; time < DashDuration; time += Time.deltaTime)
        {
            rb.linearVelocity = Vector3.Lerp(Start, End, time / DashDuration);
            yield return null;
        }
        rb.linearVelocity = End;
        yield return null;

    }
    IEnumerator WFiring(int a)
    {
        Attacking = true;
        Wp.Attack(a);
        Weapon.transform.rotation = LookDir;
        for (float i = 0; i < Wp.WD.WCoolDown[a]; i += Time.deltaTime)
        {
            Weapon.transform.position = gameObject.transform.position;
            yield return null;
        }
        Attacking = false;
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
}
