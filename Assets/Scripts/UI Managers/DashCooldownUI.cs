using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashCooldownUI : MonoBehaviour
{
    public static DashCooldownUI Instance { get; private set; }
    public Slider dashSlider;

    private EntityManager player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        dashSlider.minValue = 0;
        dashSlider.value = 0;

        player = FindAnyObjectByType<PlayerManager>();
    }

    public void TriggerDashCooldown(float duration)
    {
        dashSlider.maxValue = duration;
        dashSlider.value = duration;

        StartCoroutine(UpdateDashCooldown(duration));
    }

    private IEnumerator UpdateDashCooldown(float duration)
    {
        while (dashSlider.value > 0)
        {
            dashSlider.value -= Time.deltaTime * Time.timeScale;
            yield return null;
        }
        dashSlider.value = 0;
    }
}
