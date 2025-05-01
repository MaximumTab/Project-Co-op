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
    private string bossName;
    private static Dictionary<BaseEnemyManager, (float, float, string)> activeBosses = new Dictionary<BaseEnemyManager, (float, float, string)>();
    private static BaseEnemyManager currentDisplayedBoss;

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

    public static void RegisterBoss(BaseEnemyManager boss, float maxHP, string name)
    {
        activeBosses[boss] = (maxHP, maxHP, name);
    }

    public static void UpdateBossHealth(BaseEnemyManager boss, float currentHP)
    {
        if (activeBosses.ContainsKey(boss))
        {
            var (maxHP, _, name) = activeBosses[boss];
            activeBosses[boss] = (maxHP, currentHP, name);
            
            // Switch to this boss if it's taking damage
            if (currentHP < maxHP || currentDisplayedBoss == null)
            {
                currentDisplayedBoss = boss;
                UpdateBossUI();
            }
        }
    }

    public static void UnregisterBoss(BaseEnemyManager boss)
    {
        if (activeBosses.ContainsKey(boss))
        {
            activeBosses.Remove(boss);
            if (currentDisplayedBoss == boss)
            {
                currentDisplayedBoss = null;
                Instance[1]?.HideHealthBar();
            }
        }
    }

    private static void UpdateBossUI()
    {
        if (currentDisplayedBoss != null && activeBosses.TryGetValue(currentDisplayedBoss, out var stats))
        {
            var (maxHP, currentHP, name) = stats;
            Instance[1].maxHealth = maxHP;
            Instance[1].currentHealth = currentHP;
            Instance[1].bossName = name;
            Instance[1].UpdateHealthUI();
            Instance[1].ShowHealthBar();
        }
    }

    public static bool IsBossRegistered(BaseEnemyManager boss)
    {
        return activeBosses.ContainsKey(boss);
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
        if (isBoss) BossEntrance();
        if (currentHealth <= 0f) Die();
    }

    public void Init(float maxHP, string bossName)
    {
        maxHealth = maxHP;
        currentHealth = maxHP;
        this.bossName = bossName;

        if (bossNameText) bossNameText.text = bossName;
        UpdateHealthUI();
        isVisible = false;
        HideHealthBar();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
        if (isBoss) BossEntrance();
        if (currentHealth <= 0f) Die();
    }

    public virtual void BossEntrance()
    {
        if (!isVisible && isBoss && currentHealth < maxHealth)
        {
            ShowHealthBar();
            isVisible = true;
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider) healthSlider.value = currentHealth / maxHealth;
        if (healthText) healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        if (bossNameText && isBoss) bossNameText.text = bossName;
    }

    public void ShowHealthBar()
    {
        if (healthBarContainer) healthBarContainer.SetActive(true);
    }

    public void HideHealthBar()
    {
        if (healthBarContainer) healthBarContainer.SetActive(false);
    }

    void Die()
    {
        Debug.Log(isBoss ? $"{bossName} died." : "Player died.");
        if (isBoss)
        {
            HideHealthBar();
            isVisible = false;
        }
    }
}