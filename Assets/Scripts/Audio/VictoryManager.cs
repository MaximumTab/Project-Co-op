using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class VictoryManager : MonoBehaviour
{
    [Header("Victory Settings")]
    public AudioClip victoryMusic;
    public float volume = 1.0f;

    [Header("Audio Mixer Settings")]
    public AudioMixerGroup outputMixerGroup;

    [Header("Tracked Objects")]
    public List<GameObject> trackedObjects=new List<GameObject>();

    public List<BaseEnemyManager> trackedObjScripts=new List<BaseEnemyManager>();
    public Dictionary<BaseEnemyManager,float> TOSHp=new Dictionary<BaseEnemyManager, float>();
    public float MaxTOSHp;

    private bool musicPlayed = false;

    private void Start()
    {
        foreach (GameObject GO in trackedObjects)
        {
            trackedObjScripts.Add(GO.GetComponentInChildren<BaseEnemyManager>());
        }
    }

    void Update()
    {
        if (musicPlayed || trackedObjects.Count == 0) return;

        // Remove all null (destroyed) objects from the list
        trackedObjects.RemoveAll(obj => !obj );
        foreach (BaseEnemyManager BEM in trackedObjScripts)
        {
            TOSHp[BEM]=BEM.SM.CurHpPerc();
        }

        MaxTOSHp = TOSHp.Values.Max();

        // If all objects are destroyed, play the music
        if ((trackedObjects.Count == 0 ||MaxTOSHp<=0)&& victoryMusic )
        {
            //enemy defeated popup, move if you feel there's a better spot for this!
            NotificationManager.Instance.ShowNotification("Enemy Defeated!");
            PlayVictoryMusic();
        }
    }

    void PlayVictoryMusic()
    {
        GameObject musicPlayer = new GameObject("VictoryMusicPlayer");
        AudioSource audioSource = musicPlayer.AddComponent<AudioSource>();

        audioSource.clip = victoryMusic;
        audioSource.volume = volume;

        if (outputMixerGroup )
        {
            audioSource.outputAudioMixerGroup = outputMixerGroup;
        }

        audioSource.Play();
        Destroy(musicPlayer, victoryMusic.length);

        musicPlayed = true;
    }
}
