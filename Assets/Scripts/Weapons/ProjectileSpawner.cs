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
        Debug.Log(OnceOnHit != null);
        Proj.GetComponent<Projectile>().WC=this;
        Proj.GetComponent<Projectile>().WD=WD;
        Proj.GetComponent<Projectile>().PS=PS;
        Proj.GetComponent<Projectile>().SetCompNum(CompNum);
        StartCoroutine(ShootProjs());
    }

    IEnumerator ShootProjs()
    {
        WeaponColliderIndex = new Dictionary<Collider, int>();
        float duration=0;
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
            foreach (Collider col in TempProj.GetComponentsInChildren<Collider>())
            {
                OnceOnHit.Add(col,new List<EntityManager>());
                WeaponColliderIndex.Add(col,colliderIndex);
            }

            for (float nothingburger=0;duration<IntBetweenSpawns;duration+=Time.deltaTime)
            {
                yield return null;
            }

            duration -= IntBetweenSpawns;
        }
        yield return null;
    }
}
