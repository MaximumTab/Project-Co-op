using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;

public class EntityManager : MonoBehaviour
{
    protected Animator Anim;
    public EntityData ED;
    public Rigidbody rb{ get; private set; }
    protected Vector3 MoveDir;
    protected Quaternion LookDir;
    public Quaternion LastLook{ get; private set; }
    private bool LookCooldown = true;
    //public float TurnDuration = 0.15f;
    
    private GameObject Weapon;
    public float timeToDie;
    [SerializeField] private Weapons[] weaponsArray;
    [SerializeField] private int WeaponInUse;
    public Weapon Wp { get; private set; }
    public int Lvl;

    private float Acceleration=1000;
    public readonly float Speed=10;
    
    private bool isGrounded;
    private bool jumpCooldown=true;
    private int Jumps = 0;
    
    private float CayoteLength=0.5f;
    private float JumpWait = 0.2f;
    
    private float DashDistance = 5;
    private float BaseDashDist = 5;
    private float DashDownTime = 2;
    private float DashDuration = 0.1f;
    private bool DashCool = true;
    
    private float DropGrav = 0.5f;
    private float TerminalVel = 20f;
    
    private bool[] Attacking;
    private bool[] BusyAtk;

    public StatManager SM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        ChangeWeapon();
        SM = new StatManager();
        Anim = gameObject.GetComponentInChildren<Animator>();
        if (gameObject.GetComponent<Rigidbody>())
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }
        Jumps = ED.BonusJumps;
        SM.LevelUp(ED,Lvl);
        Lvl=SM.IncreaseLvl(Lvl);
        SM.CurAspd();
    }

    // Update is called once per frame
    public virtual void Update()
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

        StartCoroutine(AfterTimeRemove(timeToDie));
    }

    public IEnumerator AfterTimeRemove(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void ChangeWeapon()
    {
        if (Wp)
        {
            Wp.RemoveMe();
        }

        if (weaponsArray.Length > WeaponInUse)
        {
            Weapon = Instantiate(weaponsArray[WeaponInUse].Weapon, transform.parent);
        }

        if (Weapon)
        {
            Wp = Weapon.GetComponent<Weapon>();
            Wp.PS = this;
            Attacking = new bool[Wp.WD.AbilityStruct.Length];
            BusyAtk = new bool[Wp.WD.AbilityStruct.Length];
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
        if (Weapon)
        {
            (bool, int) InputAtk = AtkInput();
            if (InputAtk.Item1 && !Attacking.Max() && !BusyAtk[InputAtk.Item2])
            {
                if (Anim)
                {
                    Anim.SetTrigger("IsAttacking");
                }

                StartCoroutine(WFiring(InputAtk.Item2));
            }
        }
    }
    void Jump()
    {
        if (isJumpable())
        {
            rb.AddForce(0,ED.JumpForce,0,ForceMode.Impulse);
            SoundManager.Play3DSound(SoundType.Jump, transform, 1f, 2f, 10f);
            if (Anim)
            {
                Anim.SetTrigger("IsJumping");
            }
        }
    }
    void Drop()
    {
        if (rb)
        {
            if (rb.linearVelocity.y < -TerminalVel)
            {
                rb.AddForce(-Physics.gravity);
            }
            else if (rb.linearVelocity.y < -1)
            {
                rb.AddForce(new Vector3(0, -DropGrav, 0));
            }
        }
    }
    public virtual void Look()
    {
        if (MoveDir != Vector3.zero&&LookCooldown)
        {
            transform.rotation= Quaternion.LookRotation(MoveDir);
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
        if (rb)
        {
            Vector3 AdjustedSpeed = SpeedLimit()*Time.deltaTime;
            rb.AddForce(AdjustedSpeed * Acceleration);
        }

        if (Anim && new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude > 0.5f)
        {
            Anim.SetBool("IsWalking",true);
        }else if (Anim)
        {
            Anim.SetBool("IsWalking",false);
        }
    }

    void Dash()
    {
        if (DashInput()&& DashCool)
        {
            SoundManager.Play3DSound(SoundType.Dash, transform, 1f, 2f, 10f);
            StartCoroutine(Dashing(MoveDir));
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
    
   /* public IEnumerator LerpRotation(Quaternion target, Quaternion goal)
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
    }*/
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
    public IEnumerator Dashing(Vector3 MoveDir)
    {
        Vector3 Start = rb.linearVelocity;
        Vector3 End = MoveDir * (DashDistance * BaseDashDist);
        if (Anim)
        {
            Anim.SetTrigger("IsDashing");
        }

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
        BusyAtk[a] = true;
        if (Wp.Attack(a))
        {
            if (Anim)
            {
                Anim.SetFloat("Speed", SM.CurAspd());
                Anim.SetInteger("Attack",a);
            }

            for (float i = 0; i < (Wp.WD.AbilityStruct[a].AbilityDuration + 0.05f) / SM.CurAspd(); i += Time.deltaTime)
            {
                if (Wp.WD.AbilityStruct[a].AbilityUnInterruptDuration / SM.CurAspd() <= i)
                {
                    Attacking[a] = false;
                }

                Weapon.transform.position = gameObject.transform.position;
                Weapon.transform.rotation = LookDir;
                yield return null;
            }
        }

        if (Anim)
        {
            Anim.SetInteger("Attack", -1);
        }

        Attacking[a] = false;
        BusyAtk[a] = false;
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
     public class StatManager
     {
        public float Exp { get; private set; }
        public float Atk { get; private set; }
        public Dictionary<int,float> AtkAddBuffs=new Dictionary<int, float>();
        public Dictionary<int,float> AtkPercBuffs=new Dictionary<int, float>();
        public float Hp { get; private set; }
        public Dictionary<int, float> AddDamageReduction = new Dictionary<int, float>();
        public Dictionary<int, float> PercDamageReduction = new Dictionary<int, float>();
        public float MaxHp { get; private set; }
        public Dictionary<int,float> HpAddBuffs=new Dictionary<int, float>();
        public Dictionary<int,float> HpPercBuffs=new Dictionary<int, float>();
        public float Aspd { get; private set; }//Start on 100
        public Dictionary<int,float> AspdAddBuffs=new Dictionary<int, float>();
        public Dictionary<int,float> AspdPercBuffs=new Dictionary<int, float>();
        public float Acd { get; private set; }
        public Dictionary<int,float> AcdAddBuffs=new Dictionary<int, float>();
        public Dictionary<int,float> AcdPercBuffs=new Dictionary<int, float>();
        public void ChangeHp(float AddHp)
        {
            if (AddHp >= 0)
            {
                Hp += AddHp;
            }
            else
            {
                Hp += (AddHp-AddDamageReduction.Values.Sum()) * (1-PercDamageReduction.Values.Sum()/100);
            }

            if (Hp > MaxHp)
            {
                Hp = MaxHp;
            }
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

        public float CurAtk()
        {
            return BuffedAtk();
        }

        public float BuffedAtk()
        {
            return (Atk + AtkAddBuffs.Values.Sum()) * (1 + AtkPercBuffs.Values.Sum() / 100);
        }

        public float CurHpPerc()
        {
            return Hp / MaxHp * 100;
        }

        public float CurAspd()
        {
            return BuffedAspd() / 100;//tailored to animation speed
        }

        public float BuffedAspd()
        {
            return (Aspd + AspdAddBuffs.Values.Sum())*(1+AspdPercBuffs.Values.Sum()/100);
        }

        public float CurAcd()
        {
            return 100 / Acd;//tailored for wait seconds
        }
        public float BuffedAcd()
        {
            return (Acd + AcdAddBuffs.Values.Sum())*(1+AcdPercBuffs.Values.Sum()/100);
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
     public void DmgReductAddBuff(float Add, float Time)
     {
         StartCoroutine(AddBuff(Add, Time, SM.AddDamageReduction));
     }
     public void DmgReductPercBuff(float Add, float Time)
     {
         StartCoroutine(AddBuff(Add, Time, SM.PercDamageReduction));
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
        if (Time >=0)
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
            Jumps = ED.BonusJumps;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(CayoteTime());
    }
    [System.Serializable]
    public struct Weapons
    {
        public GameObject Weapon;
    }
}
