using UnityEngine;

public class UITESTER : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        // Health controls
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerHealthManager.Instance.TakeDamage(10f);
            Debug.Log("Took 10 damage.");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayerHealthManager.Instance.Heal(5f);
            Debug.Log("Healed 5 HP.");
        }

        // XP controls
        if (Input.GetKeyDown(KeyCode.X))
        {
            XPManager.Instance.GainXP(25f);
            Debug.Log("Gained 25 XP.");
        }
    }
}
