using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

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


    public void UpdateXPUI(float Exp,float MaxExp,float Lvl)
    {
        if (xpSlider)
        {
            xpSlider.value = Exp / MaxExp;
        }

        if (xpText)
        {
            xpText.text = $"{Mathf.FloorToInt(Exp)} / {Mathf.FloorToInt(MaxExp)}";
        }

        if (levelText)
        {
            levelText.text = $"Lvl {Lvl}";
        }
    }
}
