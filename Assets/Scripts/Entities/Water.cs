using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    public CanvasGroup waterTintUI;
    public float damagePerSecond = 5f;
    public float maxAlpha = 5f;
    public float fadeInSpeed = 1f;
    public float fadeOutSpeed = 3f;

    private bool playerInside = false;
    private Coroutine tintRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        if (tintRoutine != null)
            StopCoroutine(tintRoutine);
        tintRoutine = StartCoroutine(FadeIn());
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        EntityManager em = other.GetComponent<EntityManager>();
        if (em != null)
        {
            em.SM.ChangeHp(-damagePerSecond * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        if (tintRoutine != null)
            StopCoroutine(tintRoutine);
        tintRoutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        while (waterTintUI.alpha < maxAlpha)
        {
            waterTintUI.alpha += Time.deltaTime * fadeInSpeed;
            yield return null;
        }
        waterTintUI.alpha = maxAlpha;
    }

    private IEnumerator FadeOut()
    {
        while (waterTintUI.alpha > 0f && !playerInside)
        {
            waterTintUI.alpha -= Time.deltaTime * fadeOutSpeed;
            yield return null;
        }
        waterTintUI.alpha = 0f;
    }
}
