using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/// <summary>
/// Contains the references of the rig IK objects of the rig this is attached to.
/// 
/// For now, implementing this with functionality as the priority. It won't be scaleable for now.
/// </summary>
[RequireComponent(typeof(Rig))]
public class RigContainer : MonoBehaviour
{
    [Serializable]
    public struct Hip
    {
        public MultiParentConstraint IK;
        public PointTowards pointTowards;
        public HipDisplacement hipDisplacement;
    }

    [Serializable]
    public struct Leg
    {
        public TwoBoneIKConstraint IK;
        public KneeBehaviour knee;
    }

    [SerializeField] private Rig rigReference;
    [SerializeField] private Hip hip;
    [SerializeField] private MultiParentConstraint headIK;
    [SerializeField] private TwoBoneIKConstraint rightArmIK;
    [SerializeField] private TwoBoneIKConstraint leftArmIK;
    [SerializeField] private Leg rightLeg;
    [SerializeField] private Leg leftLeg;

    public Rig RigReference { get { return rigReference; } }
    public Hip HipReference { get { return hip; } }
    public MultiParentConstraint HeadIK { get { return headIK; } }
    public TwoBoneIKConstraint RightArmIK { get { return rightArmIK; } }
    public TwoBoneIKConstraint LeftArmIK { get { return leftArmIK; } }
    public Leg RightLeg { get { return rightLeg; } }
    public Leg LeftLeg { get { return leftLeg; } }

    public float RigWeight
    {
        get => rigReference.weight;
        set => rigReference.weight = value;
    }
}
