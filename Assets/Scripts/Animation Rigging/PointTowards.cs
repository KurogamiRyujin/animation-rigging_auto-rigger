using UnityEngine;

/// <summary>
/// Behaviour for making a transform point towards a target with its axis as the direction.
/// </summary>
public class PointTowards : MonoBehaviour
{
    public enum Direction
    {
        X,
        Y,
        Z,
        X_INVERSE,
        Y_INVERSE,
        Z_INVERSE
    }
    [SerializeField] private Transform target;
    [SerializeField] private Transform constrainedObject;
    [SerializeField] private Direction direction;

    private Vector3 initialLocalEulers;

    private void Start()
    {
        initialLocalEulers = constrainedObject.localEulerAngles;
    }

    private void LateUpdate()
    {
        constrainedObject.localEulerAngles = initialLocalEulers;
        Vector3 vector = target.localPosition - constrainedObject.localPosition;
        Vector3 rotation;

        switch (direction)
        {
            case Direction.X:
                constrainedObject.right = vector.normalized;
                break;
            case Direction.Y:
                rotation = Quaternion.FromToRotation(constrainedObject.up.normalized, vector.normalized).eulerAngles;
                constrainedObject.localEulerAngles = rotation;
                break;
            case Direction.Z:
                constrainedObject.forward = vector.normalized;
                break;
            case Direction.X_INVERSE:
                constrainedObject.right = vector.normalized;
                constrainedObject.right = -constrainedObject.right;
                break;
            case Direction.Y_INVERSE:
                rotation = Quaternion.FromToRotation(-constrainedObject.up.normalized, vector.normalized).eulerAngles;
                constrainedObject.localEulerAngles = rotation;
                break;
            case Direction.Z_INVERSE:
                constrainedObject.forward = vector.normalized;
                constrainedObject.forward = -constrainedObject.forward;
                break;
        }
    }
}
