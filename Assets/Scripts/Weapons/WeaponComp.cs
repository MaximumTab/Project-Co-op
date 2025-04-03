using System.Collections;
using UnityEngine;

public class WeaponComp : Weapon
{
    private Weapon ParentWeapon;
    public int Index;
    public override void Start()
    {
        base.Start();
        StartCoroutine(KillAfterUse());
    }

    public void GiveStats(int CN,Weapon inheritWeapon)
    {
        CompNum=CN;
        WD = inheritWeapon.WD;
        PS = inheritWeapon.PS;
        WeaponColliders =gameObject.GetComponentsInChildren<Collider>();
        ParentWeapon = inheritWeapon;
    }

    IEnumerator KillAfterUse()
    {
        Debug.Log("Deleting after "+WD.AbilityStruct[CompNum].AbilityDuration);
        yield return new WaitForSeconds(WD.AbilityStruct[CompNum].AbilityDuration);
        Destroy(ParentWeapon.CompScripts[Index]);
        Destroy(gameObject);
    }
}
