using System.Collections;
using UnityEngine;

public class MeleeGooseWeap : Weapon
{
    public override IEnumerator SpecialDuration(int i)
    {
        switch (i)
        {
            case 0 :
                yield return SliceDash(); //slash
                break;
            case 1 :
                yield return Pecking();
                break;
        }
        
    }

    public IEnumerator SliceDash()
    {
        StartCoroutine(PS.DashCoolDown());
        StartCoroutine(PS.Dashing(PS.rb.linearVelocity.normalized));  //dash
        yield return new WaitForSeconds(0.4f);  //small delay between second slash
        StartCoroutine(PS.Dashing(PS.rb.linearVelocity.normalized));  //dash again
    }

    public IEnumerator Pecking()
    {
       PS.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
       yield return new WaitForSeconds(1.5f);  //duration of pecks is 1.5
       PS.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    
    
    
}
