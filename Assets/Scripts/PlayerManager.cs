using UnityEngine;

public class PlayerManager : EntityManager
{
    [SerializeField] private Transform CameraRotation;
    private Quaternion CameraRot;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void MoveInput()
    {
        CameraRot=Quaternion.Euler(0,CameraRotation.rotation.eulerAngles.y,0);
        MoveDir =CameraRot*new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
    }
    public override (bool,int) AtkInput() //Choose how to Shoot in Child
    {
        for (int i = 1; i < 4; i++)
        {
            if (Input.GetButton("Fire" + i))
            {
                return (true, i-1);
            }
        }
        return (false,0);
    }
    public override bool JumpInput() //Choose how to Jump in Child
    {
        if (Input.GetButtonDown("Jump"))
        {
            return true;
        }
        return false;
    }
    public override bool DashInput() //Choose how to Dash in Child
    {
        if (Input.GetButtonDown("Debug Multiplier"))
        {
            return true;
        }
        return false;
    }
    public override void Look()
    {
        LookDir = CameraRotation.rotation;
        if (Attacking&&LookCooldown)
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
