using System;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject Proj;
    private Weapon WProj;

    public Weapon Wp;
    

    private void OnEnable()
    {
        GameObject TempProj=Instantiate(Proj,gameObject.transform.position,gameObject.transform.rotation);
        WProj = TempProj.GetComponent<Weapon>();
        WProj.WD = Wp.WD;
        WProj.PS = Wp.PS;
    }
}
