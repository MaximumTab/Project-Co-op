using System.Collections;
using UnityEngine;

public class LawnmowerWeap : Weapon
{
    public override IEnumerator SpecialDuration(int i)
    {
        switch (i)
        {
            case 1: // Mower Dash
                yield return PS.StartCoroutine(MowerDash());
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
        boss.isDashing = true;

        Vector3 lookDir = boss.GetTarget().position - PS.transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
        {
            PS.transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // lock in place
        PS.rb.linearVelocity = Vector3.zero;
        PS.rb.angularVelocity = Vector3.zero;
        PS.rb.constraints = RigidbodyConstraints.FreezeAll;

        Debug.Log("[MowerDash] Wind-up (3 seconds) — frozen.");
        yield return new WaitForSeconds(3f);


        // STEP 2: Get latest player position
        Vector3 finalTarget = boss.GetTarget().position;
        finalTarget.y = PS.transform.position.y;
        Vector3 dashDir = (finalTarget - PS.transform.position).normalized;

        Debug.Log($"[MowerDash] Wind-up done. Dashing toward: {finalTarget}");
        
        PS.rb.constraints = RigidbodyConstraints.FreezeRotation;


        // STEP 3: Dash 
        float dashSpeed = 75f;
        float dashDuration = 2f; 
        float timer = 0f;

        Debug.Log("[MowerDash] Dashing!");
        while (timer < dashDuration)
        {
            PS.rb.linearVelocity = dashDir * dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        // STEP 4: Stop 
        PS.rb.linearVelocity = Vector3.zero;

        Debug.Log("[MowerDash] Dash finished. Resuming normal behaviour.");

        boss.isDashing = false;
    }


}
