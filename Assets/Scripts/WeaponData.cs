using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string WName;
    public float[] WAtkPers;
    public float[] WCoolDown;
    public int WNumAtks;
    public WActiveHpPercRangeForAtks[] WAHPRFA;
    [System.Serializable]
    public struct WActiveHpPercRangeForAtks
    {
        public float LowLim;
        public float HighLim;
    }
    
}
