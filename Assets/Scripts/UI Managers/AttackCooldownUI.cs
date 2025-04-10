using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackCooldownUI : MonoBehaviour
{
    public static AttackCooldownUI Instance { get; private set; }

    public Slider[] cooldownSliders;
    public Weapon weapon;
    public EntityManager playerStats;

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
        for (int i = 0; i < cooldownSliders.Length; i++)
        {
            cooldownSliders[i].minValue = 0;
            cooldownSliders[i].value = 0;
        }
    }

    public void TriggerCooldown(int attackIndex)
    {
        if (attackIndex < 0 || attackIndex >= cooldownSliders.Length)
            return;

        float cooldownDuration = weapon.WD.WCoolDown[attackIndex] * playerStats.SM.CurAcd();
        cooldownSliders[attackIndex].maxValue = cooldownDuration;
        cooldownSliders[attackIndex].value = cooldownDuration;

        StartCoroutine(UpdateCooldown(attackIndex, cooldownDuration));
    }

    private IEnumerator UpdateCooldown(int index, float duration)
    {
        while (cooldownSliders[index].value > 0)
        {
            cooldownSliders[index].value -= Time.deltaTime;
            yield return null;
        }
        cooldownSliders[index].value = 0;
    }
}
