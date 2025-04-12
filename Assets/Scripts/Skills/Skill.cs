using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public string skillName;
    public Skill[] prerequisites;

    public bool isUnlocked = false;
    private Image image;

    public Color unlockedColor = Color.white;
    public Color lockedColor = Color.gray;

    void Start()
    {
        image = GetComponent<Image>();
        UpdateVisual();
    }

    public void TryUnlock()
    {
        if (CanUnlock())
        {
            isUnlocked = true;
            UpdateVisual();
            Debug.Log($"Unlocked {skillName}");
        }
    }

    public bool CanUnlock()
    {
        foreach (Skill prereq in prerequisites)
        {
            if (!prereq.isUnlocked)
                return false;
        }
        return true;
    }

    private void UpdateVisual()
    {
        if (image != null)
            image.color = isUnlocked ? unlockedColor : lockedColor;
    }

    // Optional: for buttons
    public void OnClick()
    {
        TryUnlock();
    }
}
