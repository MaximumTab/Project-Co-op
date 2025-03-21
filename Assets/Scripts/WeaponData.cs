using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string WName;
    public float[] WAtkPers;
    public float WCoolDown;
}
