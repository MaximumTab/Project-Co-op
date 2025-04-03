using System.Collections;
using UnityEngine;

public class WeaponComp : Weapon
{
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
    }

    IEnumerator KillAfterUse()
    {
        Debug.Log("Deleting after "+WD.WAtkDuration[CompNum]);
        yield return new WaitForSeconds(WD.WAtkDuration[CompNum]);
        Destroy(gameObject);
    }
}
