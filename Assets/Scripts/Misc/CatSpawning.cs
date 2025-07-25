using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatSpawning : MonoBehaviour
{
    private ParticleSystem Beacon;
    [SerializeField] private List<GameObject> trackedObjects;
    public List<BaseEnemyManager> trackedObjScripts=new List<BaseEnemyManager>();
    public Dictionary<BaseEnemyManager,float> TOSHp=new Dictionary<BaseEnemyManager, float>();
    public float MaxTOSHp;
    [SerializeField] private GameObject CatBossArea;
    public BaseEnemyManager CatBoss;
    [SerializeField] private float checkInterval = 0.5f;
    [SerializeField] private int sceneToLoadIndex = 1;
    private bool ReadToSpawn;
    private bool hasSpawned;
    [SerializeField] private Light ToDis;

    private float timer;

    private void Start()
    {
        foreach (GameObject GO in trackedObjects)
        {
            trackedObjScripts.Add(GO.GetComponentInChildren<BaseEnemyManager>());
        }

        Beacon = gameObject.GetComponentInChildren<ParticleSystem>();
        CatBoss = CatBossArea.GetComponentInChildren<BaseEnemyManager>();
        Beacon.Stop();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        trackedObjects.RemoveAll(obj => !obj );
        foreach (BaseEnemyManager BEM in trackedObjScripts)
        {
            TOSHp[BEM]=BEM.SM.CurHpPerc();
        }

        MaxTOSHp = TOSHp.Values.Max();
        if (timer >= checkInterval)
        {
            timer = 0f;
            trackedObjects.RemoveAll(obj => !obj);

            if ((trackedObjects.Count == 0||MaxTOSHp<=0)&&!ReadToSpawn)
            {
                ReadToSpawn = true;
                Beacon.Play();
            }else if (ReadToSpawn&&hasSpawned)
            {
                if(!CatBoss)
                {
                    SceneManager.LoadSceneAsync(sceneToLoadIndex);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&ReadToSpawn&&!hasSpawned)
        {
            GameObject CBA= Instantiate(CatBossArea,transform);
            CatBoss=CBA.GetComponentInChildren<BaseEnemyManager>();
            hasSpawned = true;
            if (ToDis)
            {
                Beacon.Stop();
                ToDis.enabled = false;
            }
        }
    }
}
