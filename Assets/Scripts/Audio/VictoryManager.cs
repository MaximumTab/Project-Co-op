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

    private bool musicPlayed = false;

    void Update()
    {
        if (musicPlayed || trackedObjects.Count == 0) return;

        // Remove all null (destroyed) objects from the list
        trackedObjects.RemoveAll(obj => !obj );

        // If all objects are destroyed, play the music
        if (trackedObjects.Count == 0 && victoryMusic )
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
