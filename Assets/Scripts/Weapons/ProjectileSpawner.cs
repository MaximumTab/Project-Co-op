using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class ProjectileSpawner : WeaponComp
{
    public GameObject Proj;

    public int AmtSpawnedPerActivate;
    public float IntBetweenSpawns;
    private int projectilesShotAtATimeAmt = 0;

    public override void Start()
    {
        base.Start();
        Proj.GetComponent<Projectile>().WC=this;
        Proj.GetComponent<Projectile>().WD=WD;
        Proj.GetComponent<Projectile>().PS=PS;
        Proj.GetComponent<Projectile>().SetCompNum(CompNum);
        StartCoroutine(ShootProjs());
    }

    IEnumerator ShootProjs()
    {
        WeaponColliderIndex = new Dictionary<Collider, int>();
        for (int i = 0; i < AmtSpawnedPerActivate; i++)
        {
            GameObject TempProj= Instantiate(Proj,gameObject.transform.position,gameObject.transform.rotation);
            TempProj.transform.SetParent(PS.transform.parent);
            TempProj.name = "TempProj" + projectilesShotAtATimeAmt;
            TempProj.GetComponent<Projectile>().SetCompNum(CompNum);
            /*TempProj.GetComponent<Projectile>().WC=this;
            TempProj.GetComponent<Projectile>().WD=WD;
            TempProj.GetComponent<Projectile>().PS=PS;*/
            
            WeaponColliders.AddRange(TempProj.GetComponentsInChildren<Collider>());
            
            yield return new WaitForSeconds(IntBetweenSpawns);
        }

        foreach (Collider col in WeaponColliders)
        {
            OnceOnHit.Add(col,new List<EntityManager>());
            WeaponColliderIndex.Add(col,colliderIndex);
        }
    }
}
