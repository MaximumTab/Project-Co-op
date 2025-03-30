using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthManager : MonoBehaviour
{
    public static BossHealthManager Instance { get; private set; }

    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isVisible = false;

    [Header("UI")]
    public Slider healthSlider;
    public TMP_Text healthText;
    public TMP_Text bossNameText;
    public GameObject healthBarContainer;

    void Awake()
    {
        Instance = this;
        HideHealthBar();
    }

    public void Init(float maxHP, string bossName)
    {
        maxHealth = maxHP;
        currentHealth = maxHP;

        if (bossNameText != null)
        {
            bossNameText.text = bossName;
        }

        UpdateHealthUI();
        isVisible = false;
        HideHealthBar();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();

        if (!isVisible)
        {
            ShowHealthBar();
            isVisible = true;
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }

    public void ShowHealthBar()
    {
        if (healthBarContainer != null)
        {
            healthBarContainer.SetActive(true);
        }
    }

    public void HideHealthBar()
    {
        if (healthBarContainer != null)
        {
            healthBarContainer.SetActive(false);
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        HideHealthBar();
        isVisible = false;
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }
}
