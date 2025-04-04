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


    

    private void Update()
    {
        if (OnceOnHit.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Proj.GetComponent<Projectile>().WC=this;
        Proj.GetComponent<Projectile>().WD=WD;
        Proj.GetComponent<Projectile>().PS=PS;
        StartCoroutine(ShootProjs());
    }

    IEnumerator ShootProjs()
    {
        WeaponColliderIndex = new Dictionary<Collider, int>();
        for (int i = 0; i < AmtSpawnedPerActivate; i++)
        {
            GameObject TempProj= Instantiate(Proj,gameObject.transform.position,gameObject.transform.rotation);
            TempProj.GetComponent<Projectile>().WC=this;
            TempProj.GetComponent<Projectile>().WD=WD;
            TempProj.GetComponent<Projectile>().PS=PS;
            
            WeaponColliders.AddRange(TempProj.GetComponentsInChildren<Collider>());
            foreach (Collider col in WeaponColliders)
            {
                WeaponColliderIndex.Add(col,colliderIndex);
            }
            yield return new WaitForSeconds(IntBetweenSpawns);
        }
    }
}
