using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class LawnmowerWeap : Weapon
{
    public override IEnumerator SpecialDuration(int i)
    {
        switch (i)
        {
            case 1: // Mower Dash
                yield return PS.StartCoroutine(MowerDash());
                break;

            case 2: // Grass Clipping Spin
                yield return PS.StartCoroutine(MowerSpin());
                break;
        }

        yield return base.SpecialDuration(i);
    }


    private IEnumerator MowerDash()
    {
        LawnmowerBossManager boss = PS as LawnmowerBossManager;
        if (boss == null || boss.GetTarget() == null)
        {
            Debug.LogWarning("[MowerDash] Boss or target is null — aborting.");
            yield break;
        }

        Debug.Log("[MowerDash] Dash selected — entering wind-up phase.");

        // STEP 1: freeze
        Vector3 lookDir = boss.GetTarget().position - PS.transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
        {
            PS.transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // lock in place
        PS.rb.linearVelocity = Vector3.zero;
        PS.rb.angularVelocity = Vector3.zero;
        PS.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Debug.Log("[MowerDash] Wind-up (3 seconds) — frozen.");
        yield return new WaitForSeconds(1f);
        Debug.Log("[MowerDash] Wind-up (2 seconds) — frozen.");
        yield return new WaitForSeconds(1f);
        Debug.Log("[MowerDash] Wind-up (1 seconds) — frozen.");
        yield return new WaitForSeconds(1f);  

        PS.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

       // StartCoroutine(PS.Dashing(PS.transform.forward));
        StartCoroutine(PS.DashCoolDown()); // start cooldown
        yield return PS.Dashing(PS.transform.forward); // wait for dash to finish
    }

    private IEnumerator MowerSpin()
    {
        LawnmowerBossManager boss = PS as LawnmowerBossManager;
        if (boss == null)
        {
            Debug.LogWarning("[MowerSpin] Boss is null — aborting.");
            yield break;
        }

        Debug.Log("[MowerSpin] Starting spin.");
        
        // Stop and freeze movement
        PS.rb.linearVelocity = Vector3.zero;
        PS.rb.angularVelocity = Vector3.zero;
        PS.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        StartCoroutine(PS.DashCoolDown());
        
        float duration = WD.AbilityStruct[2].AbilityDuration;
        float timer = 0f;

        while (timer < duration)
        {
            PS.transform.Rotate(Vector3.up * 720 * Time.deltaTime); // spinning visual
            timer += Time.deltaTime;
            yield return null;
        }

        // Restore constraints to allow movement again
        PS.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        Debug.Log("[MowerSpin] Spin finished.");
    }

}
