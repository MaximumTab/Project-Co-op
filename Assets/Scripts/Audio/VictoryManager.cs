using System.Collections.Generic;
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
    public List<GameObject> trackedObjects = new List<GameObject>();

    private static bool musicPlayed = false;

    void Update()
    {
        if (!musicPlayed)
        {
            
            trackedObjects.RemoveAll(obj => obj == null);

            if (trackedObjects.Count == 0 && victoryMusic != null)
            {
                PlayVictoryMusic();
            }
        }
    }

    void PlayVictoryMusic()
    {
        GameObject musicPlayer = new GameObject("VictoryMusicPlayer");
        AudioSource audioSource = musicPlayer.AddComponent<AudioSource>();

        audioSource.clip = victoryMusic;
        audioSource.volume = volume;

       
        if (outputMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = outputMixerGroup;
        }

        audioSource.Play();
        Destroy(musicPlayer, victoryMusic.length);
        musicPlayed = true;
    }
}
