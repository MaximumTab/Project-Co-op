using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillParent : MonoBehaviour
{
    public static SkillParent Instance;
    public List<Skill>[] SkillTree;
    [SerializeField] TMP_Text Skillpointui;
    [SerializeField] public int Skillpoints = 0;
    [SerializeField] private GameObject skillPointIndicator;
    [SerializeField] private TMP_Text equippedText;
    
    [SerializeField] private List<Image> mageImages;
    [SerializeField] private List<Image> tankImages;
    [SerializeField] private List<Image> warriorImages;

    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color unlockedColor = Color.white;


    public int CurrentBranch { get; private set; } = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        Instance = this;
        SkillTree = new List<Skill>[4];
        SkillTree[0] = new List<Skill>();
        SkillTree[1] = new List<Skill>();
        SkillTree[2] = new List<Skill>();
        SkillTree[3] = new List<Skill>();
        GetAllSkills();
        SkillpointText();
        UpdateBranchImages();
    }


    void GetAllSkills()
    {
        foreach (Skill tempskill in gameObject.GetComponentsInChildren<Skill>())
        {
            Debug.Log(tempskill.name);
            SkillTree[tempskill.branch].Add(tempskill);
        }
    }

    public void ChangeSkillPoints(int sp)
    {
        Skillpoints -= sp;
    }

    public int GetSkillPoints()
    {
        return Skillpoints;
    }

    public void SkillpointText()
    {
        Skillpointui.text = "Unlock Tokens: " + Skillpoints;

        if (skillPointIndicator != null)
            skillPointIndicator.SetActive(Skillpoints > 0);
    }

    public void ChangeBranch(int i)
    {
        CurrentBranch = i;
        for (int a = 1; a < 4; a++)
        {
            foreach (Skill getskill in SkillTree[a])
            {
                if (a == i)
                {
                    getskill.OnEquip();
                }
                else
                {
                    getskill.OnRemove();
                }
            }
        }
        UpdateBranchImages();
    }

    private void UpdateBranchImages()
    {
        void SetImageStates(List<Image> images, bool active)
        {
            foreach (var img in images)
            {
                img.color = active ? unlockedColor : lockedColor;
            }
        }

        bool isMage = CurrentBranch == 1;
        bool isTank = CurrentBranch == 2;
        bool isWarrior = CurrentBranch == 3;

        SetImageStates(mageImages, isMage);
        SetImageStates(tankImages, isTank);
        SetImageStates(warriorImages, isWarrior);
    }
    public void UpdateEquippedText(string className)
    {
        if (equippedText != null)
            equippedText.text = $"Equipped: {className}";
    }

}
