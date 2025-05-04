using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;

public enum SoundType
{
    Jump,
    Dash,
    WarriorSlice,
    WarriorWhirlwind,
    WarriorBuff,
    FireballCast,
    FireballExplosion,
    ASPDBUFF,
    Projectile,
    VICTORYTHEME,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixerGroup;
    }

    /// <summary>
    /// Plays a 3D sound attached to the specified parent.
    /// </summary>
    public static void Play3DSound(SoundType sound, Transform parent, float spatialBlend = 1f, float minDistance = 1f, float maxDistance = 50f, float volume = 1f)
    {
        if (instance == null)
        {
            Debug.LogWarning("SoundManager instance not found.");
            return;
        }

        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning($"No AudioClips assigned for SoundType: {sound}");
            return;
        }

        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        GameObject soundObject = new GameObject("3D_Sound_" + sound);
        soundObject.transform.SetParent(parent, false);
        soundObject.transform.localPosition = Vector3.zero;

        AudioSource newAudioSource = soundObject.AddComponent<AudioSource>();
        newAudioSource.clip = randomClip;
        newAudioSource.spatialBlend = spatialBlend;
        newAudioSource.rolloffMode = AudioRolloffMode.Linear;
        newAudioSource.minDistance = minDistance;
        newAudioSource.maxDistance = maxDistance;
        newAudioSource.volume = volume;

        if (instance.audioMixerGroup != null)
            newAudioSource.outputAudioMixerGroup = instance.audioMixerGroup;

        newAudioSource.Play();
        instance.StartCoroutine(instance.DestroyAfterPlay(newAudioSource));
    }

    private IEnumerator DestroyAfterPlay(AudioSource source)
    {
        if (source == null || source.clip == null)
            yield break;

        float waitTime = source.clip.length;

        // Wait until playback finishes or the source becomes null/destroyed
        float timer = 0f;
        while (timer < waitTime)
        {
            if (source == null || source.gameObject == null)
                yield break;

            timer += Time.deltaTime;
            yield return null;
        }

        if (source != null && source.gameObject != null)
        {
            Destroy(source.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds => sounds;

    [HideInInspector] public string name;

    [SerializeField]
    private AudioClip[] sounds;
}
