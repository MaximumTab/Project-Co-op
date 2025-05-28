using UnityEngine;

[System.Serializable]
public class LegIK
{
    public Transform target;         // Target position for the paw
    public Transform[] bones;        // Bones: Upper Leg, Lower Leg, Paw
}

public class CatLegIKController : MonoBehaviour
{
    [Header("Leg Objects")]
    public LegIK frontLeftLeg;
    public LegIK frontRightLeg;
    public LegIK backLeftLeg;
    public LegIK backRightLeg;

    [Header("Tail Objects")]
    public LegIK tail;

    [Header("Head Objects")]
    public Transform headBone;
    public Transform headTarget;
    public float headLookSpeed = 5f;
    public Vector3 headRotationOffsetEuler;

    public int iterations = 10;
    public float threshold = 0.01f;

    void LateUpdate()
    {
        SolveLegIK(frontLeftLeg);
        SolveLegIK(frontRightLeg);
        SolveLegIK(backLeftLeg);
        SolveLegIK(backRightLeg);
        SolveLegIK(tail);

        SolveHeadLookAt();
    }

    void SolveLegIK(LegIK leg)
    {
        if (leg.target == null || leg.bones == null || leg.bones.Length == 0)
        {
            return;
        }
        
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < leg.bones.Length - 1; j++)
            {
                Transform currentBone = leg.bones[j];

                Vector3 toEndEffector = leg.bones[leg.bones.Length - 1].position - currentBone.position;
                Vector3 toTarget = leg.target.position - currentBone.position;

                Quaternion rotation = Quaternion.FromToRotation(toEndEffector, toTarget);
                currentBone.rotation = rotation * currentBone.rotation;

                if ((leg.bones[leg.bones.Length - 1].position - leg.target.position).sqrMagnitude < threshold * threshold)
                {
                    return;
                }
            }
        }
    }
    
     void SolveHeadLookAt()
    {
        if (headBone == null || headTarget == null)
        {
            return;
        }

        Vector3 direction = headTarget.position - headBone.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, headBone.up);

        Quaternion offsetRotation = Quaternion.Euler(headRotationOffsetEuler);
        targetRotation *= offsetRotation;

        headBone.rotation = Quaternion.Slerp(headBone.rotation, targetRotation, Time.deltaTime * headLookSpeed);
    }
}
