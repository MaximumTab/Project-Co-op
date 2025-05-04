using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WinTemp : MonoBehaviour
{
    [SerializeField] private List<GameObject> trackedObjects;
    [SerializeField] private float checkInterval = 0.5f;
    [SerializeField] private int sceneToLoadIndex = 1;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            trackedObjects.RemoveAll(obj => obj == null);

            if (trackedObjects.Count == 0)
            {
                SceneManager.LoadSceneAsync(sceneToLoadIndex);
                enabled = false;
            }
        }
    }
}
