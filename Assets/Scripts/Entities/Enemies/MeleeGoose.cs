using UnityEngine;

public class MeleeGoose : BaseEnemyManager
{
    [SerializeField] private float distance;
    
    public override (bool, int) AtkInput()
    {
        float targetDistance = (transform.position - target.position).magnitude;

        if (AtkDelay)
        {
            return (false,-1);
        }
        StartCoroutine(DecisionDelay());
        if (targetDistance < distance)
        {
            return (true, 1);
        }
        else
        {
            return (true, 0);
        }
    }
    
    
    
    
    
}
