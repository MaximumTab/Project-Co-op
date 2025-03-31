using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackCooldownUI : MonoBehaviour
{
    public Slider[] cooldownSliders; // Make sure this matches the number of attacks
    private Weapon weapon;
    private EntityManager playerStats; // Reference to the player EntityManager

    private void Start()
    {
        for (int i = 0; i < cooldownSliders.Length; i++)
        {
            cooldownSliders[i].minValue = 0;
            cooldownSliders[i].value = 0;
        }

        playerStats = FindAnyObjectByType<PlayerManager>();
        weapon = playerStats.Wp;
    }

    public void TriggerCooldown(int attackIndex)
    {
        if (attackIndex < 0 || attackIndex >= cooldownSliders.Length)
            return;

        float cooldownDuration = playerStats.Wp.WD.WCoolDown[attackIndex] * playerStats.SM.CurAcd();
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
