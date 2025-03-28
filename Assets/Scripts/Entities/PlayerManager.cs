using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : EntityManager
{
    [SerializeField] private Transform CameraRotation;
    private Quaternion CameraRot;
    private InputSystem_Actions input;
    private Vector2 moveInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        input = new InputSystem_Actions();
        input.Enable();
    }

    public override void MoveInput()
    {
        moveInput = input.Player.Move.ReadValue<Vector2>();
        CameraRot=Quaternion.Euler(0,CameraRotation.rotation.eulerAngles.y,0);
        MoveDir =CameraRot*new Vector3(moveInput.x,0, moveInput.y);
    }
    public override (bool,int) AtkInput() //Choose how to Shoot in Child
    {
        if (input.Player.Attack1.IsPressed())
            return (true, 0); 
        if (input.Player.Attack2.IsPressed())
            return (true, 1);
        if (input.Player.Attack3.IsPressed())
            return (true, 2);
        
        return (false,0);
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
        if (Attacking.Max()&&LookCooldown)
        {
            StartCoroutine(LerpRotation(transform.rotation, CameraRot));
        }else if (MoveDir != Vector3.zero&&LookCooldown)
        {
            StartCoroutine(LerpRotation(transform.rotation, Quaternion.LookRotation(MoveDir)));
        }
        else
        {
            transform.rotation = LastLook;
        }
    }
}
