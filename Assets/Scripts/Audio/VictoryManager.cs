using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [Header("Victory Settings")]
    public AudioClip victoryMusic;
    public float volume = 1.0f;

    [Header("Tracked Objects")]
    public List<GameObject> trackedObjects = new List<GameObject>();

    private static bool musicPlayed = false;

    void Update()
    {
        if (!musicPlayed)
        {
            // Remove any null (destroyed) objects
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
        audioSource.Play();

        Destroy(musicPlayer, victoryMusic.length);
        musicPlayed = true;
    }
}
