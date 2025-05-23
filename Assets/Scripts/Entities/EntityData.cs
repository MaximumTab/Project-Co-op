using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Entity/EntityData")]
public class EntityData : ScriptableObject
{
    public string Name;
    public bool isBoss;
    
    public float BaseAtk;//initial attack at base lvl
    public float GrowAtk;//additive increase of attack per level
    
    public float BaseHp;//initial Hp at base lvl
    public float GrowHp;//additive increase of hp per level

    public float BaseAspd;//initial percentile attack speed on base lvl 
    public float GrowAspd;//additive increase in percentile attack speed

    public float BaseAC;//initial percentile of Ability cooldowns on base lvl
    public float GrowAC;//additive increase of percentile cooldowns

    public float BaseExp;//for players the baseExp is the initial max exp. for enemies this is the base exp given when defeated.
    public float GrowExp;//Multiplies with BaseExp depending on lvl
    
    public float Speed=10;
    public float JumpForce = 10;
    public int BonusJumps = 0;
    public float DashDistance = 5;
    public float BaseDashDist = 5;
    public float DashDownTime = 2;
    public float DashDuration = 0.1f;
}
