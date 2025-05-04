using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public string skillName;
    public Skill[] prerequisites;

    public bool isUnlocked = false;
    private Image image;

    private bool isequiped = false;

    public Color unlockedColor = Color.white;
    public Color lockedColor = Color.gray;
    [SerializeField] private int sprequirement;


    [SerializeField] public int branch;

    [SerializeField] private bool changeweapon;

    void Start()
    {
        image = GetComponent<Image>();
    
        if (branch == 0 )
        {
            isequiped = true;
            isUnlocked = true;
        }
        UpdateVisual();
    }

    public void TryUnlock()
    {
        if (isUnlocked)
            return;

        if (CanUnlock())
        {
            isUnlocked = true;
            OnEquip();
            SkillParent.Instance.ChangeSkillPoints(sprequirement);
            SkillParent.Instance.SkillpointText();
        }
        else
        {
            GetComponent<SkillTooltipTrigger>()?.tooltipManager.ShowErrorTooltip("Not Enough Points");
        }
    }
    public bool CanUnlock()
    {
        foreach (Skill prereq in prerequisites)
        {
            if (!prereq.isUnlocked || !prereq.isequiped)
                return false;
        }
        if (sprequirement > SkillParent.Instance.GetSkillPoints())
            return false;
        return true;
    }

    private void UpdateVisual()
    {
        if (image != null)
            image.color = isequiped ? unlockedColor : lockedColor;
    }

    public void OnClick()
    {
        TryEquip();
        if (isUnlocked)
        {
             SkillParent.Instance.ChangeBranch(branch);
        }
    }

    public void TryEquip()
    {
        if (!isequiped && isUnlocked)
        {
            OnEquip();
        }
        else if (!isequiped)
        {
            TryUnlock();
        }
    }

    public void ChangeWeapon()
    {
        FindAnyObjectByType<PlayerManager>().ChangeWeapon(branch);
    }

    public void OnEquip()
    {
        if (isUnlocked)
        {
            if (changeweapon)
            {
                ChangeWeapon();
            }
            isequiped = true;
            UpdateVisual();
        }
    }
       public void OnRemove()
    {
            isequiped = false;
            UpdateVisual();
    }
}
