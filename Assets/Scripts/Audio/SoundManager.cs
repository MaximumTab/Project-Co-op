using UnityEngine;
using System;
using UnityEngine.Audio;

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
    Projectile

}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixerGroup;
    }


    public static void Play3DSound(SoundType sound, Transform parent, float spatialBlend = 1f, float minDistance = 1f, float maxDistance = 50f, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        
        GameObject soundObject = new GameObject("3D_Sound");
        soundObject.transform.SetParent(parent, false); // Spawn under the triggering object
        soundObject.transform.localPosition = Vector3.zero; // Ensure it's positioned correctly relative to the parent
        
        AudioSource newAudioSource = soundObject.AddComponent<AudioSource>();
        newAudioSource.clip = randomClip;
        newAudioSource.spatialBlend = spatialBlend; // 3D sound
        newAudioSource.rolloffMode = AudioRolloffMode.Linear;
        newAudioSource.minDistance = minDistance;
        newAudioSource.maxDistance = maxDistance;
        newAudioSource.volume = volume; //volume 

          
        if (instance.audioMixerGroup != null)
        newAudioSource.outputAudioMixerGroup = instance.audioMixerGroup;

        newAudioSource.Play();
        instance.StartCoroutine(instance.DestroyAfterPlay(newAudioSource));
    }

    private System.Collections.IEnumerator DestroyAfterPlay(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source.gameObject);
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
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
