using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string WName;
    public SerializedDictionary<int,float[]> WAtkPers;
    public float[] WAtkDuration;
    public float[] WAtkInterruptDuration;
    public bool[] WIsInterruptable;
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
