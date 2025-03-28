using System;
using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject Proj;
    private Weapon WProj;

    public Weapon Wp;

    public int AmtSpawnedPerActivate;
    public float IntBetweenSpawns;


    void Start()
    {
        Proj.GetComponent<Weapon>().WD=Wp.WD;
        Proj.GetComponent<Weapon>().PS=Wp.PS;
    }

    private void OnEnable()
    {
        StartCoroutine(ShootProjs());
    }

    IEnumerator ShootProjs()
    {
        for (int i = 0; i < AmtSpawnedPerActivate; i++)
        {
            Instantiate(Proj,gameObject.transform.position,gameObject.transform.rotation);
            yield return new WaitForSeconds(IntBetweenSpawns);
        }
    }
}
