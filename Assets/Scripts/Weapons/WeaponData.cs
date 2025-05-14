using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string WName;
    public WStruct[] AbilityStruct;
    [System.Serializable]
    public struct WStruct
    {
        public float LowHpLim;
        public float HighHpLim;
        public float[] AbilityPercentages;
        public float[] KnockBackStrengths;
        public float[] KnockUpStrengths;
        public float AbilityDuration;
        public float AbilityUnInterruptDuration;
        public bool IsInterruptable;
        public float AbilityCooldown;
    }

    public float AbStructHpHighMax()
    {
        float MaxHighHp = 0;
        foreach (WStruct AbStruct in AbilityStruct)
        {
            if (AbStruct.HighHpLim > MaxHighHp)
            {
                MaxHighHp = AbStruct.HighHpLim;
            }
        }

        return MaxHighHp;
    }

}
