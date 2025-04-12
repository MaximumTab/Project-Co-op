using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthManager : MonoBehaviour
{
    public static HealthManager[] Instance = new HealthManager[2];
    [SerializeField] private int UITarget;
    
    [SerializeField] private bool isBoss;
    private float maxHealth;
    private float currentHealth;
    private bool isVisible;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private GameObject healthBarContainer;
    
    void Awake()
    {
        Instance[UITarget] = this;
        if (isBoss)
        {
            HideHealthBar();
        }
    }

    public void SetHp(float Hp)
    {
        maxHealth = Hp;
        currentHealth = Hp;
        UpdateHealthUI();
    }

    public void SetCurHp(float Hp)
    {
        currentHealth = Hp;
        UpdateHealthUI();
        BossEntrance();
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Init(float maxHP, string bossName)
    {
        maxHealth = maxHP;
        currentHealth = maxHP;

        if (bossNameText)
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
        BossEntrance();
        if (currentHealth <= 0f)
        {
            Die();
        }
        
    }

    public virtual void BossEntrance()
    {
        if (!isVisible&&isBoss&&currentHealth<maxHealth)
        {
            ShowHealthBar();
            isVisible = true;
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider)
        {
            healthSlider.value = currentHealth / maxHealth;
        }

        if (healthText)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }
    public void ShowHealthBar()
    {
        if (healthBarContainer)
        {
            healthBarContainer.SetActive(true);
        }
    }
    

    public void HideHealthBar()
    {
        if (healthBarContainer)
        {
            healthBarContainer.SetActive(false);
        }
    }
    void Die()
    {
        Debug.Log("Player died.");
        if (isBoss)
        {
            HideHealthBar();
            isVisible = false;
        }

        // Add death logic here
    }
}
