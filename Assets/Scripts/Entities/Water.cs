using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    public CanvasGroup waterTintUI;
    public float damagePerSecond = 5f;
    public float maxAlpha = 0.3f;
    public float fadeInSpeed = 1f;
    public float fadeOutSpeed = 3f;
    public float rampUpScale = 1f;
    public float afterTimeInc=2f;

    private bool playerInside = false;
    private Coroutine tintRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        if (tintRoutine != null)
            StopCoroutine(tintRoutine);
        tintRoutine = StartCoroutine(FadeIn());
        StartCoroutine(DamageInc());
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        EntityManager em = other.GetComponent<EntityManager>();
        if (em)
        {
            em.SM.ChangeHp(-damagePerSecond*rampUpScale * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        if (tintRoutine != null)
            StopCoroutine(tintRoutine);
        tintRoutine = StartCoroutine(FadeOut());
        StopCoroutine(DamageInc());
        rampUpScale = 1;
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

    private IEnumerator DamageInc()
    {
        yield return null;
        for (int a = 0; a < 10; a++)
        {
            for (float i = 0; i < afterTimeInc; i += Time.deltaTime)
            {
                yield return null;
            }

            rampUpScale += 1;
            yield return null;
        }

    }
}
