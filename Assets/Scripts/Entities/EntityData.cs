using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Entity/EntityData")]
public class EntityData : ScriptableObject
{
    public string Name;
    
    public float BaseAtk;
    public float GrowAtk;
    
    public float BaseHp;
    public float GrowHp;

    public float BaseAspd;
    public float GrowAspd;

    public float BaseAC;
    public float GrowAC;
}
