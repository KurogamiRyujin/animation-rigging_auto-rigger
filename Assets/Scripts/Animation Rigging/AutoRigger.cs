using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AutoRigger : MonoBehaviour
{
    private string HIP = "Armature/Hips";
    private string SPINE_HIERARCHY => HIP + "/Spine/Spine1/Spine2";// Hierarchy reaching to the Head and Arms.
    private string HEAD_NAME => SPINE_HIERARCHY + "/Neck/Head";
    private string RIGHT_ARM_NAME => SPINE_HIERARCHY + "/RightShoulder/RightArm";
    // private  string RIGHT_FOREARM_NAME = "RightForeArm";
    // private  string RIGHT_HAND_NAME = "RightHand"; 
    private string LEFT_ARM_NAME => SPINE_HIERARCHY + "/LeftShoulder/LeftArm";
    // private  string LEFT_FOREARM_NAME = "LeftForeArm";
    // private  string LEFT_HAND_NAME = "LeftHand"; 
    private string RIGHT_UP_LEG_NAME => HIP + "/RightUpLeg";
    // private  string RIGHT_LEG_NAME => RIGHT_UP_LEG_NAME + "/RightLeg";
    // private  string RIGHT_FOOT_NAME => RIGHT_LEG_NAME + "/RightFoot";
    private string LEFT_UP_LEG_NAME => HIP + "/LeftUpLeg";
    // private  string LEFT_LEG_NAME => LEFT_UP_LEG_NAME + "/LeftLeg";
    // private  string LEFT_FOOT_NAME => LEFT_LEG_NAME + "/LeftFoot";

    [SerializeField] private RigContainer dynamicRigPrefab;
    [SerializeField] private RuntimeAnimatorController animatorController;

    private Animator animator;
    private RigContainer rigContainer = null;

    public void SetupRig(GameObject assignedAvatar)
    {
        List<SkinnedMeshRenderer> renderers = new List<SkinnedMeshRenderer>(assignedAvatar.GetComponentsInChildren<SkinnedMeshRenderer>());

        animator = assignedAvatar.GetComponent<Animator>();
        animator.enabled = false;
        animator.applyRootMotion = false;
        animator.runtimeAnimatorController = animatorController;

        assignedAvatar.TryGetComponent(out RigBuilder rigBuilder);

        if (!rigBuilder)
        {
            rigBuilder = assignedAvatar.AddComponent<RigBuilder>();
        }

        rigContainer = Instantiate<RigContainer>(dynamicRigPrefab, assignedAvatar.transform);

        rigBuilder.layers.Add(new RigLayer(rigContainer.RigReference));

        rigContainer.HipReference.IK.data.constrainedObject = assignedAvatar.transform.Find(HIP);

        rigContainer.HeadIK.data.constrainedObject = assignedAvatar.transform.Find(HEAD_NAME);

        rigContainer.RightArmIK.data.root = assignedAvatar.transform.Find(RIGHT_ARM_NAME);
        rigContainer.RightArmIK.data.mid = rigContainer.RightArmIK.data.root.GetComponentsInChildren<Transform>()[1];// GetComponentsInChildren includes the parent, so we start at 1
        rigContainer.RightArmIK.data.tip = rigContainer.RightArmIK.data.mid.GetComponentsInChildren<Transform>()[1];// Same as above

        rigContainer.LeftArmIK.data.root = assignedAvatar.transform.Find(LEFT_ARM_NAME);
        rigContainer.LeftArmIK.data.mid = rigContainer.LeftArmIK.data.root.GetComponentsInChildren<Transform>()[1];
        rigContainer.LeftArmIK.data.tip = rigContainer.LeftArmIK.data.mid.GetComponentsInChildren<Transform>()[1];

        rigContainer.RightLeg.IK.data.root = assignedAvatar.transform.Find(RIGHT_UP_LEG_NAME);
        rigContainer.RightLeg.IK.data.mid = rigContainer.RightLeg.IK.data.root.GetComponentsInChildren<Transform>()[1];
        rigContainer.RightLeg.IK.data.tip = rigContainer.RightLeg.IK.data.mid.GetComponentsInChildren<Transform>()[1];
        rigContainer.RightLeg.knee.SetFoot(rigContainer.RightLeg.IK.data.tip);

        rigContainer.LeftLeg.IK.data.root = assignedAvatar.transform.Find(LEFT_UP_LEG_NAME);
        rigContainer.LeftLeg.IK.data.mid = rigContainer.LeftLeg.IK.data.root.GetComponentsInChildren<Transform>()[1];
        rigContainer.LeftLeg.IK.data.tip = rigContainer.LeftLeg.IK.data.mid.GetComponentsInChildren<Transform>()[1];
        rigContainer.LeftLeg.knee.SetFoot(rigContainer.LeftLeg.IK.data.tip);

        rigContainer.HipReference.IK.data.sourceObjects[0].transform.position = rigContainer.HipReference.IK.data.constrainedObject.position;
        rigContainer.HipReference.IK.data.sourceObjects[0].transform.rotation = rigContainer.HipReference.IK.data.constrainedObject.rotation;

        rigContainer.HeadIK.data.sourceObjects[0].transform.position = rigContainer.HeadIK.data.constrainedObject.position;
        rigContainer.HeadIK.data.maintainPositionOffset = true;
        rigContainer.HeadIK.data.sourceObjects[0].transform.rotation = rigContainer.HeadIK.data.constrainedObject.rotation;

        // rigContainer.RightArmIK.data.hint.SetPositionAndRotation(rigContainer.RightArmIK.data.mid.position + new Vector3(-0.7f, -0.8f, 0.3f), rigContainer.RightArmIK.data.mid.rotation);
        // rigContainer.RightArmIK.data.hint.localPosition = rigContainer.RightArmIK.data.mid.localPosition + new Vector3(1f, -0.8f, -0.4f);
        // rigContainer.RightArmIK.data.hint.localRotation = rigContainer.RightArmIK.data.mid.localRotation;
        // rigContainer.RightArmIK.data.target.SetPositionAndRotation(rigContainer.RightArmIK.data.tip.position, rigContainer.RightArmIK.data.tip.rotation);
        rigContainer.RightArmIK.data.target.position = rigContainer.RightArmIK.data.tip.position;
        rigContainer.RightArmIK.data.target.rotation = rigContainer.RightArmIK.data.tip.rotation;

        // rigContainer.LeftArmIK.data.hint.SetPositionAndRotation(rigContainer.LeftArmIK.data.mid.position + new Vector3(0.7f, -0.8f, 0.3f), rigContainer.LeftArmIK.data.mid.rotation);
        // rigContainer.LeftArmIK.data.hint.localPosition = rigContainer.LeftArmIK.data.mid.localPosition + new Vector3(-1f, -0.8f, -0.4f);
        // rigContainer.LeftArmIK.data.hint.localRotation = rigContainer.LeftArmIK.data.mid.localRotation;
        // rigContainer.LeftArmIK.data.target.SetPositionAndRotation(rigContainer.LeftArmIK.data.tip.position, rigContainer.LeftArmIK.data.tip.rotation);
        rigContainer.LeftArmIK.data.target.position = rigContainer.LeftArmIK.data.tip.position;
        rigContainer.LeftArmIK.data.target.rotation = rigContainer.LeftArmIK.data.tip.rotation;

        // rigContainer.RightLegIK.data.hint.localPosition = new Vector3(rigContainer.RightLegIK.data.mid.localPosition.x, rigContainer.RightLegIK.data.mid.localPosition.y, 0.5f);
        // rigContainer.RightLegIK.data.hint.localRotation = Quaternion.identity;
        rigContainer.RightLeg.IK.data.target.position = rigContainer.RightLeg.IK.data.tip.position;
        rigContainer.RightLeg.IK.data.target.rotation = rigContainer.RightLeg.IK.data.tip.rotation;

        // rigContainer.LeftLegIK.data.hint.localPosition = new Vector3(rigContainer.LeftLegIK.data.mid.localPosition.x, rigContainer.LeftLegIK.data.mid.localPosition.y, 0.5f);
        // rigContainer.LeftLegIK.data.hint.localRotation = Quaternion.identity;
        rigContainer.LeftLeg.IK.data.target.position = rigContainer.LeftLeg.IK.data.tip.position;
        rigContainer.LeftLeg.IK.data.target.rotation = rigContainer.LeftLeg.IK.data.tip.rotation;

        rigBuilder.Build();

        rigContainer.RightLeg.knee.Init();
        rigContainer.LeftLeg.knee.Init();

        // fix feet pinning
        //rigContainer.RightLeg.IK.data.target.localPosition = new Vector3(1, -0.5f, 0.5f); //rigContainer.RightLeg.IK.data.tip.localPosition;
        //rigContainer.RightLeg.IK.data.target.localEulerAngles = Vector3.zero; //rigContainer.RightLeg.IK.data.tip.localRotation; 
        //rigContainer.LeftLeg.IK.data.target.localPosition = new Vector3(-1, -0.5f, 0.5f); //rigContainer.LeftLeg.IK.data.tip.localPosition;
        //rigContainer.LeftLeg.IK.data.target.localEulerAngles = Vector3.zero; //rigContainer.LeftLeg.IK.data.tip.localRotation;

        animator.enabled = true;

    }
}
