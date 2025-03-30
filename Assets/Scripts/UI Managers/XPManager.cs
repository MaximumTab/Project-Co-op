using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

    [Header("XP Settings")]
    public int currentLevel = 1;
    public float currentXP = 0f;
    public float xpToNextLevel = 100f;
    public float xpMultiplier = 1.2f; 

    [Header("UI")]
    public Slider xpSlider;
    public TMP_Text xpText;
    public TMP_Text levelText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UpdateXPUI();
    }

    public void GainXP(float amount)
    {
        currentXP += amount;

        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }

        UpdateXPUI();
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;

        xpToNextLevel *= xpMultiplier;
    }

    void UpdateXPUI()
    {
        if (xpSlider)
        {
            xpSlider.value = currentXP / xpToNextLevel;
        }

        if (xpText)
        {
            xpText.text = $"{Mathf.FloorToInt(currentXP)} / {Mathf.FloorToInt(xpToNextLevel)}";
        }

        if (levelText)
        {
            levelText.text = $"Lvl {currentLevel}";
        }
    }

    public int GetLevel()
    {
        return currentLevel;
    }

    public float GetXPPercent()
    {
        return currentXP / xpToNextLevel;
    }
}
