using System;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject Proj;
    private Weapon WProj;

    public Weapon Wp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject TempProj=Instantiate(Proj);
        TempProj.transform.position = gameObject.transform.position;
        WProj = TempProj.GetComponent<Weapon>();
        WProj.WD = Wp.WD;
        WProj.PS = Wp.PS;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //Create new Projectile
    }
}
