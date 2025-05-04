using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkillParent : MonoBehaviour
{
    public static SkillParent Instance;
    public List<Skill>[] SkillTree;
    [SerializeField]TMP_Text Skillpointui;
    [SerializeField]public int Skillpoints = 0;
    [SerializeField] private GameObject skillPointIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        Instance = this;
        SkillTree = new List<Skill> [4];
        SkillTree[0] = new List<Skill>();
        SkillTree[1] = new List<Skill>();
        SkillTree[2] = new List<Skill>();
        SkillTree[3] = new List<Skill>();
        GetAllSkills();
        SkillpointText();
    }

    
    void GetAllSkills()
    {
        foreach (Skill tempskill in gameObject.GetComponentsInChildren<Skill>())
        {
            Debug.Log (tempskill.name);
            SkillTree[tempskill.branch].Add (tempskill);
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
        Skillpointui.text = "Skillpoint: " + Skillpoints;
        
        if (skillPointIndicator != null)
            skillPointIndicator.SetActive(Skillpoints > 0);
    }

    public void ChangeBranch(int i)
    {
        for (int a = 1; a < 4; a++)
        {   
            Debug.Log (SkillTree[a].Count);
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
    }
}
