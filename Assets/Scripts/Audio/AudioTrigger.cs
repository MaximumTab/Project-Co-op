using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject linkedPrefab;
    public float fadeDuration = 2f;

    private Coroutine currentFade;

    private void Update()
    {
        if (linkedPrefab == null && audioSource.isPlaying)
        {
            if (currentFade != null) StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeOut(audioSource, fadeDuration));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentFade != null) StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeIn(audioSource, fadeDuration));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentFade != null) StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeOut(audioSource, fadeDuration));
        }
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
