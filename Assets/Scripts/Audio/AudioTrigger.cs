using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject linkedPrefab;
    public float fadeDuration = 2f;
    public int priority = 0;

    private Coroutine currentFade;
    private static AudioTrigger currentActiveTrigger;

    private void Start()
    {
        audioSource.Play();
        audioSource.Stop();
    }

    private void Update()
    {
        if (!linkedPrefab && audioSource.isPlaying)
        {
            if (this != currentActiveTrigger)
            {
                if (currentFade != null) StopCoroutine(currentFade);
                currentFade = StartCoroutine(FadeOut(audioSource, fadeDuration));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentActiveTrigger == null || priority > currentActiveTrigger.priority)
            {
                if (currentActiveTrigger != null)
                    currentActiveTrigger.FadeOutSelf();

                currentActiveTrigger = this;
                if (currentFade != null) StopCoroutine(currentFade);
                currentFade = StartCoroutine(FadeIn(audioSource, fadeDuration));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && currentActiveTrigger == this)
        {
            currentActiveTrigger = null;
            if (currentFade != null) StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeOut(audioSource, fadeDuration));
        }
    }

    private void FadeOutSelf()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeOut(audioSource, fadeDuration));
    }

    private IEnumerator FadeIn(AudioSource source, float duration)
    {
        source.volume = 0f;
        if (!source.isPlaying) source.Play();

        while (source.volume < 1f)
        {
            source.volume += Time.deltaTime / duration;
            yield return null;
        }

        source.volume = 1f;
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;

        while (source.volume > 0f)
        {
            source.volume -= Time.deltaTime / duration;
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
    }
}
