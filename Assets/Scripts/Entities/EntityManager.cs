using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;

public class EntityManager : MonoBehaviour
{
    private Animator Anim;
    public EntityData ED;
    public Rigidbody rb;
    public Vector3 MoveDir;
    public Quaternion LookDir;
    public Quaternion LastLook;
    public bool LookCooldown=true;
    public float TurnDuration = 0.25f;
    
    [SerializeField] private GameObject Weapon;
    private Weapon Wp;
    public int Lvl;

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
    
    public bool[] Attacking;

    public StatManager SM=new StatManager();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        Anim = gameObject.GetComponentInParent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        Jumps = BonusJumps;
        if (Weapon != null)
        {
            Wp = Weapon.GetComponent<Weapon>();
            Attacking = new bool[Wp.WD.WNumAtks];
        }
        SM.LevelUp(ED,Lvl);
        Lvl=SM.IncreaseLvl(Lvl);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Shoot();
        Look();
        Drop();
        LevelingUp();
        if (SM.Hp <= 0)
        {
            OnDeath();
        }
    }

    public virtual void AddXP(float xp)
    {
        
    }

    public virtual void LevelingUp()
    {
        
    }

    public virtual void OnDeath()
    {
        if (Anim)
        {
            Anim.SetBool("Death", true);
        }
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
        (bool, int) InputAtk=AtkInput();
        if (InputAtk.Item1&&!Attacking.Max())
        {
            StartCoroutine(WFiring(InputAtk.Item2));
        }
    }
    void Jump()
    {
        if (isJumpable())
        {
            rb.AddForce(0,JumpForce,0,ForceMode.Impulse);
            SoundManager.Play3DSound(SoundType.Jump, transform, 1f, 2f, 10f);
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
            SoundManager.Play3DSound(SoundType.Dash, transform, 1f, 2f, 10f);
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
        Attacking[a] = true;
        if (Wp.Attack(a))
        {
            AttackCooldownUI uiCooldown = FindObjectOfType<AttackCooldownUI>();
            if (uiCooldown)
            {
                uiCooldown.TriggerCooldown(a);
            }
            for (float i = 0; i < (Wp.WD.WAtkDuration[a] + 0.05f) / SM.CurAspd(); i += Time.deltaTime)
            {
                Weapon.transform.position = gameObject.transform.position;
                Weapon.transform.rotation = LookDir;
                yield return null;
            }
        }

        Attacking[a] = false;
        yield return null;
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
     public struct StatManager
     {
        public float Exp { get; private set; }
        public float Atk { get; private set; }
        public Dictionary<int,float> AtkAddBuffs;
        public Dictionary<int,float> AtkPercBuffs; 
        public float Hp { get; private set; }
        public float MaxHp { get; private set; }
        public Dictionary<int,float> HpAddBuffs;
        public Dictionary<int,float> HpPercBuffs; 
        public float Aspd { get; private set; }//Start on 100
        public Dictionary<int,float> AspdAddBuffs;
        public Dictionary<int,float> AspdPercBuffs;
        public float Acd { get; private set; }
        public Dictionary<int,float> AcdAddBuffs;
        public Dictionary<int,float> AcdPercBuffs;
        public void ChangeHp(float AddHp)
        {
            Hp += AddHp;
        }
        public void LevelUp(EntityData ED, int Lvl)
        {
            Exp = ED.BaseExp * ED.GrowExp * Lvl + ED.BaseExp;
            Hp = ED.BaseHp + ED.GrowHp * Lvl;
            MaxHp = ED.BaseHp + ED.GrowHp * Lvl;
            Atk = ED.BaseAtk + ED.GrowAtk * Lvl;
            Aspd = ED.BaseAspd + ED.GrowAspd * Lvl;
            Acd = ED.BaseAC + ED.GrowAC * Lvl;
        }

        public int IncreaseLvl(int Lvl)
        {
            return Lvl + 1;
        }

        public float CurHpPerc()
        {
            return Hp / MaxHp * 100;
        }

        public float CurAspd()
        {
            return Aspd / 100;//tailored to animation speed
        }
        public float CurAcd()
        {
            return 100 / Acd;//tailored for wait seconds
        }

        public int FindEmptyKey(Dictionary<int,float> BuffList)
        {
            for (int i = 0; i < BuffList.Count; i++)
            {
                if (!BuffList.ContainsKey(i))
                {
                    return i;
                }
            }
            return BuffList.Count;
        }
    }
    public void HpAddBuff(float Add, float Time)
    {
        StartCoroutine(AddBuff(Add, Time, SM.HpAddBuffs));
    }
    public void HpPercBuff(float Add, float Time)
    {
        StartCoroutine(AddBuff(Add, Time, SM.HpPercBuffs));
    }
    public void AtkAddBuff(float Add, float Time)
    {
        StartCoroutine(AddBuff(Add, Time, SM.AtkAddBuffs));
    }
    public void AtkPercBuff(float Add, float Time)
    {
        StartCoroutine(AddBuff(Add, Time, SM.AtkPercBuffs));
    }
    public void AspdAddBuff(float Add, float Time)
    {
        StartCoroutine(AddBuff(Add, Time, SM.AspdAddBuffs));
    }
    public void AspdPercBuff(float Add, float Time)
    {
        StartCoroutine(AddBuff(Add, Time, SM.AspdPercBuffs));
    }
    
    IEnumerator AddBuff(float Add, float Time,Dictionary<int,float> BuffList)//Set Time to -1 for infinite buff
    {
        int BuffLoc=SM.FindEmptyKey(BuffList);
        BuffList.Add(BuffLoc,Add);
        if (Time != -1)
        {
            yield return new WaitForSeconds(Time);
            BuffList.Remove(BuffLoc);
        }
        yield return null;
    }
    private void OnTriggerStay(Collider other)
    {
        if (jumpCooldown&&!other.gameObject.transform.IsChildOf(gameObject.transform.parent))
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
