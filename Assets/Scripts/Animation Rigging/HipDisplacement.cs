using System;
using UnityEngine;

/// <summary>
/// Behaviour for displacing the hip target IK based on a given target.
/// Target is usually the head IK.
/// </summary>
public class HipDisplacement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform hint;
    [SerializeField] private Transform hip;
    [SerializeField][Range(0f, 1f)] private float hipToFloorTolerance; // Ratio of the distance between hip and floor before the hint acquires bias.
    [SerializeField] private float targetToHipYTolerance;
    [SerializeField] private float targetToFloorTolerance = 0.6f;
    [SerializeField] private AnimationCurve curve;

    private Vector3 displacement;
    private float displacementDistance;
    private float hipFloorDistanceInitial;
    private float targetFloorDistanceInitial;

    private void OnEnable()
    {
        displacementDistance = Vector3.Distance(target.position, hip.position);
        hipFloorDistanceInitial = hip.position.y - transform.position.y;
        targetFloorDistanceInitial = target.position.y - transform.position.y;
        CalculateHipDisplacement();
    }

    private void LateUpdate()
    {
        if (Math.Abs(Vector3.Distance(target.position, hip.position) - displacementDistance) > Mathf.Epsilon)
        {
            CalculateHipDisplacement();
            hip.position = target.position - displacement;
        }
    }

    private void CalculateDisplacement()
    {
        // Handle Y tolerance.
        float targetHipYDifference = target.position.y - hip.position.y;
        // If the Y property between target.position and hip.position goes below the tolerance,
        if (targetHipYDifference <= targetToHipYTolerance)
        {
            // ... displace the hip based on the tolerance value.
            hip.position = new Vector3(hip.position.x, target.position.y - targetToHipYTolerance, hip.position.z);
        }

        float hipFloorDistance = hip.position.y - transform.position.y;
        float hipFloorRatio = hipFloorDistance / hipFloorDistanceInitial;
        // Calculate hint bias, the percent it takes in the displacement formula.
        float hintBias = (hipFloorRatio >= hipToFloorTolerance) ? 0f : 1f - (hipFloorRatio / hipToFloorTolerance);
        if (hintBias > 1f) hintBias = 1f;
        if (hintBias < 0f) hintBias = 0f;

        // Handle backwards movement.
        float backwardsAngle = Vector3.SignedAngle(transform.up.normalized, (target.position - hip.position).normalized, hip.right.normalized);
        // float hintAngle = Vector3.SignedAngle(transform.up.normalized, (hint.position - transform.position).normalized, transform.right.normalized);
        // float backwardAndHintAngleRatio = backwardsAngle / hintAngle;

        //Check target to floor Y distance.
        float YDifference = target.position.y - transform.position.y;
        float YRatio = YDifference / targetFloorDistanceInitial;
        float straighteningBias = (backwardsAngle <= 0f && YRatio >= targetToFloorTolerance) ? YRatio : YRatio - hintBias;
        if (backwardsAngle > 0f) straighteningBias = 0f;
        if (straighteningBias < 0f) straighteningBias = 0f;
        if (straighteningBias > 1f) straighteningBias = 1f;
        // if(backwardAndHintAngleRatio > 0f && backwardAndHintAngleRatio < 0.5f && YRatio > targetToFloorTolerance) {
        //     displacement = (target.position - (target.position + Vector3.down)).normalized * displacementDistance;
        // }

        displacement = (((target.position - hip.position).normalized * (1 - hintBias) + (target.position - hint.position).normalized * hintBias) * displacementDistance) * (1 - straighteningBias)
                        + ((target.position - (target.position + Vector3.down)).normalized * displacementDistance) * straighteningBias;
    }

    /// <summary>
    /// Alternative method for hip behaviour.
    /// </summary>
    private void CalculateHipDisplacement()
    {
        // Closer target is to origin, stronger the hint bias.
        // Farther target is from hint, stronger midpoint bias.
        // Midpoint is the natural tendency.
        float currentTargetOriginDistance = Vector3.Distance(target.position, transform.position);
        Vector3 currentMidpoint = (target.position + transform.position) / 2;

        float hintBias = 0f;

        // Ratio by how much is the original distance between target and origin.
        float currentTargetOriginRatio = currentTargetOriginDistance / targetFloorDistanceInitial;
        // If the ratio falls below the tolerance, steadily increase hint bias.
        hintBias = curve.Evaluate(currentTargetOriginRatio);
        // Debug.Log("Curve Eval: " + curve.Evaluate(currentTargetOriginRatio) + " from " + currentTargetOriginRatio);
        // if(currentTargetOriginRatio < targetToFloorTolerance) {
        //     hintBias = 1 - (currentTargetOriginRatio / targetToFloorTolerance);
        // }

        // Calculate displacement.
        displacement = (target.position - transform.position).normalized * (1 - hintBias) + (target.position - hint.position).normalized * hintBias;
        displacement = displacement.normalized * displacementDistance;
    }
}
