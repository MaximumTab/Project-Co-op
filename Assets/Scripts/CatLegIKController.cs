using UnityEngine;

[System.Serializable]
public class LegIK
{
    public Transform target;
    public Transform[] bones;
    public Transform bendHint;
    [HideInInspector] public Vector3 defaultLocalPos;
    [HideInInspector] public Quaternion[] defaultRotations;
}

public class CatLegIKController : MonoBehaviour
{
    [Header("Leg Bones")]
    public LegIK frontLeftLeg;
    public LegIK frontRightLeg;
    public LegIK backLeftLeg;
    public LegIK backRightLeg;

    [Header("Tail IK")]
    public LegIK tail;

    [Header("Head LookAt")]
    public Transform headBone;
    public Transform headTarget;
    public float headLookSpeed = 5f;
    [SerializeField] private Vector3 headRotationOffsetEuler;

    [Header("Walk Cycle")]
    public float stepSpeed = 1f;
    public float stepHeight = 0.02f;
    public float stepLength = 0.03f;
    public float ellipseAngleDegrees = 0f;

    [Header("Body Bobbing")]
    public Transform bodyRoot;     
    public float bodyBobAmount = 0.01f;  
    public float bodyBobSpeedMultiplier = 1f;  


    [Header("Jump Control")]
    public bool isJumping = false;

    [Header("IK calculation frequency")]
    public int iterations = 10;

    [Header("How far from the target the IK will stop calculating the IK (more - more precise placement at the cost of CPU / less - less precise paw placement at the benefit of CPU)")]
    public float threshold = 0.01f;

    [Header("Rotation Reset Frequency")]
    public int resetAfterCycles = 1;

    // Cycle counters for each leg
    private int frontLeftCycleCount = 0;
    private int frontRightCycleCount = 0;
    private int backLeftCycleCount = 0;
    private int backRightCycleCount = 0;
    private int frontLeftPhaseCount = 0;
    private int frontRightPhaseCount = 0;
    private int backLeftPhaseCount = 0;
    private int backRightPhaseCount = 0;


    void Start()
    {
        frontLeftLeg.defaultLocalPos = frontLeftLeg.target.localPosition;
        frontRightLeg.defaultLocalPos = frontRightLeg.target.localPosition;
        backLeftLeg.defaultLocalPos = backLeftLeg.target.localPosition;
        backRightLeg.defaultLocalPos = backRightLeg.target.localPosition;

        SaveDefaultRotations(frontLeftLeg);
        SaveDefaultRotations(frontRightLeg);
        SaveDefaultRotations(backLeftLeg);
        SaveDefaultRotations(backRightLeg);
    }

    void SaveDefaultRotations(LegIK leg)
    {
        leg.defaultRotations = new Quaternion[leg.bones.Length];
        for (int i = 0; i < leg.bones.Length; i++)
        {
            leg.defaultRotations[i] = leg.bones[i].localRotation;
        }
    }

    void LateUpdate()
    {
        if (!isJumping)
        {
            AnimateFootTargets();
            AnimateBodyBobbing();
        }

        SolveLegIK(frontLeftLeg);
        SolveLegIK(frontRightLeg);
        SolveLegIK(backLeftLeg);
        SolveLegIK(backRightLeg);
        SolveLegIK(tail);

        SolveHeadLookAt();
    }


    void AnimateFootTargets()
    {
        float phaseTime = Time.time * stepSpeed * Mathf.PI * 2f;

        AnimateLeg(frontLeftLeg, phaseTime, 0f, ref frontLeftCycleCount, ref frontLeftPhaseCount);
        AnimateLeg(backRightLeg, phaseTime, 0f, ref backRightCycleCount, ref backRightPhaseCount);
        AnimateLeg(frontRightLeg, phaseTime, Mathf.PI, ref frontRightCycleCount, ref frontRightPhaseCount);
        AnimateLeg(backLeftLeg, phaseTime, Mathf.PI, ref backLeftCycleCount, ref backLeftPhaseCount);
    }

    void AnimateBodyBobbing()
    {
        if (bodyRoot == null) return;

        float bobPhase = Time.time * stepSpeed * bodyBobSpeedMultiplier * Mathf.PI * 2f;
        float bobOffset = Mathf.Sin(bobPhase) * bodyBobAmount;

        Vector3 newPosition = bodyRoot.localPosition;
        newPosition.y = bobOffset;  
        bodyRoot.localPosition = newPosition;
    }



    void AnimateLeg(LegIK leg, float time, float phaseOffset, ref int cycleCount, ref int phaseCount)
    {
        float phase = time + phaseOffset;

        Vector3 ellipsePoint = new Vector3(stepLength * Mathf.Cos(phase), stepHeight * Mathf.Sin(phase), 0f);
        Quaternion rotation = Quaternion.AngleAxis(ellipseAngleDegrees, Vector3.up);
        Vector3 rotatedOffset = rotation * ellipsePoint;

        leg.target.localPosition = leg.defaultLocalPos + rotatedOffset;

        if (phase - phaseCount * (2 * Mathf.PI) >= 2 * Mathf.PI)
        {
            phaseCount++;
            cycleCount++;

            if (resetAfterCycles > 0 && cycleCount % resetAfterCycles == 0)
            {
                ResetToDefaultRotation(leg);
            }
        }
    }


    void ResetToDefaultRotation(LegIK leg)
    {
        for (int i = 0; i < leg.bones.Length; i++)
        {
            leg.bones[i].localRotation = leg.defaultRotations[i];
        }
    }

    void SolveLegIK(LegIK leg)
    {
        if (leg.target == null || leg.bones == null || leg.bones.Length == 0)
            return;

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
                    return;
            }
        }

        if (leg.bendHint != null && leg.bones.Length == 3)
        {
            Vector3 rootPos = leg.bones[0].position;
            Vector3 kneePos = leg.bones[1].position;
            Vector3 footPos = leg.bones[2].position;

            Vector3 legDir = (footPos - rootPos).normalized;
            Vector3 bendDir = (leg.bendHint.position - kneePos).normalized;

            Vector3 desiredNormal = Vector3.Cross(legDir, bendDir).normalized;

            Vector3 currentDir = (kneePos - rootPos).normalized;
            Vector3 projectedDir = Vector3.ProjectOnPlane(currentDir, desiredNormal).normalized;

            float upperLength = (kneePos - rootPos).magnitude;
            leg.bones[1].position = rootPos + projectedDir * upperLength;
        }
    }

    void SolveHeadLookAt()
    {
        if (headBone == null || headTarget == null)
            return;

        Vector3 direction = headTarget.position - headBone.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, headBone.up);
        Quaternion offsetRotation = Quaternion.Euler(headRotationOffsetEuler);
        targetRotation *= offsetRotation;

        headBone.rotation = Quaternion.Slerp(headBone.rotation, targetRotation, Time.deltaTime * headLookSpeed);
    }
}
