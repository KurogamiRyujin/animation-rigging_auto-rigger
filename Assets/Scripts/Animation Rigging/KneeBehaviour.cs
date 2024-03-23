using UnityEngine;

/// <summary>
/// Behaviour for controlling the knees' hints whenever the avatar orients itself into a crouch or out of it.
/// </summary>
public class KneeBehaviour : MonoBehaviour
{
    [SerializeField] private Transform kneeHint;
    [SerializeField] private Transform Hinthint;
    [SerializeField] private float hintHeightRatioPlacement;// Ratio that determines which percentile of the target's height should the hints be positioned.
    [SerializeField] private float hintForwardDisplacement;// Value for how much the hints should be pushed forward from the target-origin axis.
    [SerializeField] private Transform target;// Reference transform that will be evaluated by the curve to determine how much the hints should be adjusted.
    [SerializeField] private Transform origin;// Reference to the origin point that will be compared to the target transform.
    [SerializeField] private AnimationCurve curve;

    private float targetOriginDifferenceInitial;
    private bool initialized = false;

    public void Init()
    {
        initialized = true;
        targetOriginDifferenceInitial = Vector3.Distance(target.position, origin.position);
    }

    private void Update()
    {
        if (initialized) CalculateDisplacement();
    }

    private void CalculateDisplacement()
    {
        float currentTargetOriginDifference = Vector3.Distance(target.position, origin.position);
        Vector3 targetOriginAxis = (target.position - origin.position).normalized;

        Vector3 verticalDisplacement = hintHeightRatioPlacement * targetOriginAxis;

        Vector3 forwardDirection = Vector3.Cross(targetOriginAxis, origin.right).normalized;

        // Lower the target goes, the farther the side displacement will be
        float sideDisplacementBias = 0f;
        float currentTargetOriginRatio = currentTargetOriginDifference / targetOriginDifferenceInitial;
        sideDisplacementBias = curve.Evaluate(currentTargetOriginRatio);

        transform.position = origin.position + verticalDisplacement;

        transform.position += ((Hinthint.position - transform.position) * sideDisplacementBias) + ((forwardDirection * hintForwardDisplacement) * (1 - sideDisplacementBias));

        // Debug.DrawRay(transform.position, forwardDirection, Color.cyan);
    }

    public void SetFoot(Transform foot)
    {
        origin = foot;
    }
}
