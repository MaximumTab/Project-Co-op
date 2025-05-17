using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerManager : EntityManager
{
    [SerializeField] private Transform CameraRotation;
    private Quaternion CameraRot;
    private InputSystem_Actions input;
    private Vector2 moveInput;
    public float Exp=0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        if (HealthManager.Instance[0])
        {
            HealthManager.Instance[0].SetHp(SM.MaxHp);
        }
        input = InputManager.Instance.Actions;
        if (XPManager.Instance)
        {
            XPManager.Instance.UpdateXPUI(Exp, SM.Exp, Lvl);
        }
    }

    public override void Update()
    {
        base.Update();
        if (HealthManager.Instance[0])
        {
            HealthManager.Instance[0].SetCurHp(SM.Hp);
        }
    }

    public override void LevelingUp()
    {
        if (Exp >= SM.Exp)
        {
            Exp -= SM.Exp;
            SM.LevelUp(ED,Lvl);
            Lvl = SM.IncreaseLvl(Lvl);
            if (XPManager.Instance)
            {
                XPManager.Instance.UpdateXPUI(Exp, SM.Exp, Lvl);
            }

            if (SkillParent.Instance)
            {
                SkillParent.Instance.ChangeSkillPoints(-1);
                SkillParent.Instance.SkillpointText();
            }

            if (HealthManager.Instance[0])
            {
                HealthManager.Instance[0].SetHp(SM.MaxHp);
            }
        }
    }

    public override void AddXP(float Xp)
    {
        Exp += Xp;
        if (XPManager.Instance)
        {
            XPManager.Instance.UpdateXPUI(Exp, SM.Exp, Lvl);
        }
    }

    public override void MoveInput()
    {
        if(Time.timeScale != 0)
        {
            moveInput = input.Player.Move.ReadValue<Vector2>();
            CameraRot = Quaternion.Euler(0, CameraRotation.rotation.eulerAngles.y, 0);
            RaycastHit hit;
            if (rb.SweepTest(CameraRot * new Vector3(moveInput.x, 0, 0), out hit, Mathf.Abs(moveInput.x)/2))
            {
                moveInput.x = 0;
            }
            if (rb.SweepTest(CameraRot * new Vector3(0, 0, moveInput.y), out hit, Mathf.Abs(moveInput.y)/2))
            {
                moveInput.y = 0;
            }
            MoveDir = CameraRot * new Vector3(moveInput.x, 0, moveInput.y);
        }
      
    }
    public override (bool,int) AtkInput() //Choose how to Shoot in Child
    {
        if (AtkDelay)
        {
            return (false,-1);
        }
        if (input.Player.Attack1.IsPressed())
            return (true, 0); 
        if (input.Player.Attack2.IsPressed())
            return (true, 1);
        if (input.Player.Attack3.IsPressed())
            return (true, 2);
        
        return (false,-1);
    }
    public override bool JumpInput() //Choose how to Jump in Child
    {
        if (input.Player.Jump.triggered)
        {
            return true;
        }
        return false;
    }
    public override bool DashInput() //Choose how to Dash in Child
    {
        if (input.Player.Sprint.triggered)
        {
            return true;
        }
        return false;
    }
    public override void Look()
    {
        LookDir = CameraRotation.rotation;
        transform.rotation= CameraRot;
    }
    

    public override void OnDeath()
    {
        base.OnDeath();
        Time.timeScale = 1; 
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(2); 
    }

}
