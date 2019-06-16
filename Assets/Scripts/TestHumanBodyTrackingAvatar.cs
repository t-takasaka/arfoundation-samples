using System;
using System.Text;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using Object = UnityEngine.Object;

public class TestHumanBodyTrackingAvatar : MonoBehaviour
{
    // 3D joint skeleton
    enum JointIndices
    {
        Invalid = -1,
        root = 0, // parent: <none> [-1]
        hips_joint = 1, // parent: root [0]
        left_upLeg_joint = 2, // parent: hips_joint [1]
        left_leg_joint = 3, // parent: left_upLeg_joint [2]
        left_foot_joint = 4, // parent: left_leg_joint [3]
        left_toes_joint = 5, // parent: left_foot_joint [4]
        left_toesEnd_joint = 6, // parent: left_toes_joint [5]
        right_upLeg_joint = 7, // parent: hips_joint [1]
        right_leg_joint = 8, // parent: right_upLeg_joint [7]
        right_foot_joint = 9, // parent: right_leg_joint [8]
        right_toes_joint = 10, // parent: right_foot_joint [9]
        right_toesEnd_joint = 11, // parent: right_toes_joint [10]
        spine_1_joint = 12, // parent: hips_joint [1]
        spine_2_joint = 13, // parent: spine_1_joint [12]
        spine_3_joint = 14, // parent: spine_2_joint [13]
        spine_4_joint = 15, // parent: spine_3_joint [14]
        spine_5_joint = 16, // parent: spine_4_joint [15]
        spine_6_joint = 17, // parent: spine_5_joint [16]
        spine_7_joint = 18, // parent: spine_6_joint [17]
        right_shoulder_1_joint = 19, // parent: spine_7_joint [18]
        right_shoulder_2_joint = 20, // parent: right_shoulder_1_joint [19]
        right_arm_joint = 21, // parent: right_shoulder_2_joint [20]
        right_forearm_joint = 22, // parent: right_arm_joint [21]
        right_hand_joint = 23, // parent: right_forearm_joint [22]
        right_handThumbStart_joint = 24, // parent: right_hand_joint [23]
        right_handThumb_1_joint = 25, // parent: right_handThumbStart_joint [24]
        right_handThumb_2_joint = 26, // parent: right_handThumb_1_joint [25]
        right_handThumbEnd_joint = 27, // parent: right_handThumb_2_joint [26]
        right_handIndexStart_joint = 28, // parent: right_hand_joint [23]
        right_handIndex_1_joint = 29, // parent: right_handIndexStart_joint [28]
        right_handIndex_2_joint = 30, // parent: right_handIndex_1_joint [29]
        right_handIndex_3_joint = 31, // parent: right_handIndex_2_joint [30]
        right_handIndexEnd_joint = 32, // parent: right_handIndex_3_joint [31]
        right_handMidStart_joint = 33, // parent: right_hand_joint [23]
        right_handMid_1_joint = 34, // parent: right_handMidStart_joint [33]
        right_handMid_2_joint = 35, // parent: right_handMid_1_joint [34]
        right_handMid_3_joint = 36, // parent: right_handMid_2_joint [35]
        right_handMidEnd_joint = 37, // parent: right_handMid_3_joint [36]
        right_handRingStart_joint = 38, // parent: right_hand_joint [23]
        right_handRing_1_joint = 39, // parent: right_handRingStart_joint [38]
        right_handRing_2_joint = 40, // parent: right_handRing_1_joint [39]
        right_handRing_3_joint = 41, // parent: right_handRing_2_joint [40]
        right_handRingEnd_joint = 42, // parent: right_handRing_3_joint [41]
        right_handPinkyStart_joint = 43, // parent: right_hand_joint [23]
        right_handPinky_1_joint = 44, // parent: right_handPinkyStart_joint [43]
        right_handPinky_2_joint = 45, // parent: right_handPinky_1_joint [44]
        right_handPinky_3_joint = 46, // parent: right_handPinky_2_joint [45]
        right_handPinkyEnd_joint = 47, // parent: right_handPinky_3_joint [46]
        left_shoulder_1_joint = 48, // parent: spine_7_joint [18]
        left_shoulder_2_joint = 49, // parent: left_shoulder_1_joint [48]
        left_arm_joint = 50, // parent: left_shoulder_2_joint [49]
        left_forearm_joint = 51, // parent: left_arm_joint [50]
        left_hand_joint = 52, // parent: left_forearm_joint [51]
        left_handThumbStart_joint = 53, // parent: left_hand_joint [52]
        left_handThumb_1_joint = 54, // parent: left_handThumbStart_joint [53]
        left_handThumb_2_joint = 55, // parent: left_handThumb_1_joint [54]
        left_handThumbEnd_joint = 56, // parent: left_handThumb_2_joint [55]
        left_handIndexStart_joint = 57, // parent: left_hand_joint [52]
        left_handIndex_1_joint = 58, // parent: left_handIndexStart_joint [57]
        left_handIndex_2_joint = 59, // parent: left_handIndex_1_joint [58]
        left_handIndex_3_joint = 60, // parent: left_handIndex_2_joint [59]
        left_handIndexEnd_joint = 61, // parent: left_handIndex_3_joint [60]
        left_handMidStart_joint = 62, // parent: left_hand_joint [52]
        left_handMid_1_joint = 63, // parent: left_handMidStart_joint [62]
        left_handMid_2_joint = 64, // parent: left_handMid_1_joint [63]
        left_handMid_3_joint = 65, // parent: left_handMid_2_joint [64]
        left_handMidEnd_joint = 66, // parent: left_handMid_3_joint [65]
        left_handRingStart_joint = 67, // parent: left_hand_joint [52]
        left_handRing_1_joint = 68, // parent: left_handRingStart_joint [67]
        left_handRing_2_joint = 69, // parent: left_handRing_1_joint [68]
        left_handRing_3_joint = 70, // parent: left_handRing_2_joint [69]
        left_handRingEnd_joint = 71, // parent: left_handRing_3_joint [70]
        left_handPinkyStart_joint = 72, // parent: left_hand_joint [52]
        left_handPinky_1_joint = 73, // parent: left_handPinkyStart_joint [72]
        left_handPinky_2_joint = 74, // parent: left_handPinky_1_joint [73]
        left_handPinky_3_joint = 75, // parent: left_handPinky_2_joint [74]
        left_handPinkyEnd_joint = 76, // parent: left_handPinky_3_joint [75]
        neck_1_joint = 77, // parent: spine_7_joint [18]
        neck_2_joint = 78, // parent: neck_1_joint [77]
        neck_3_joint = 79, // parent: neck_2_joint [78]
        neck_4_joint = 80, // parent: neck_3_joint [79]
        head_joint = 81, // parent: neck_4_joint [80]
        jaw_joint = 82, // parent: head_joint [81]
        chin_joint = 83, // parent: jaw_joint [82]
        nose_joint = 84, // parent: head_joint [81]
        right_eye_joint = 85, // parent: head_joint [81]
        right_eyeUpperLid_joint = 86, // parent: right_eye_joint [85]
        right_eyeLowerLid_joint = 87, // parent: right_eye_joint [85]
        right_eyeBall_joint = 88, // parent: right_eye_joint [85]
        left_eye_joint = 89, // parent: head_joint [81]
        left_eyeUpperLid_joint = 90, // parent: left_eye_joint [89]
        left_eyeLowerLid_joint = 91, // parent: left_eye_joint [89]
        left_eyeBall_joint = 92, // parent: left_eye_joint [89]
    }

    //Current joint index, Child joint index, Current HumanBodyBones, Parent HumanBodyBones
    (JointIndices, JointIndices, HumanBodyBones, HumanBodyBones)[] boneMaps =
    {
        (JointIndices.hips_joint, JointIndices.spine_1_joint, HumanBodyBones.Hips, HumanBodyBones.Hips),
        (JointIndices.left_upLeg_joint, JointIndices.left_leg_joint, HumanBodyBones.LeftUpperLeg, HumanBodyBones.Hips),
        (JointIndices.left_leg_joint, JointIndices.left_foot_joint, HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftUpperLeg),
        (JointIndices.left_foot_joint, JointIndices.left_toes_joint, HumanBodyBones.LeftFoot, HumanBodyBones.LeftLowerLeg),
        (JointIndices.left_toes_joint, JointIndices.left_toesEnd_joint, HumanBodyBones.LeftToes, HumanBodyBones.LeftFoot),
        (JointIndices.right_upLeg_joint, JointIndices.right_leg_joint, HumanBodyBones.RightUpperLeg, HumanBodyBones.Hips),
        (JointIndices.right_leg_joint, JointIndices.right_foot_joint, HumanBodyBones.RightLowerLeg, HumanBodyBones.RightUpperLeg),
        (JointIndices.right_foot_joint, JointIndices.right_toes_joint, HumanBodyBones.RightFoot, HumanBodyBones.RightLowerLeg),
        (JointIndices.right_toes_joint, JointIndices.right_toesEnd_joint, HumanBodyBones.RightToes, HumanBodyBones.RightFoot),
        (JointIndices.spine_1_joint, JointIndices.spine_4_joint, HumanBodyBones.Spine, HumanBodyBones.Hips),
        (JointIndices.spine_4_joint, JointIndices.spine_7_joint, HumanBodyBones.Chest, HumanBodyBones.Spine),
        (JointIndices.spine_7_joint, JointIndices.neck_1_joint, HumanBodyBones.UpperChest, HumanBodyBones.Chest),
        (JointIndices.right_shoulder_1_joint, JointIndices.right_arm_joint, HumanBodyBones.RightShoulder, HumanBodyBones.UpperChest),
        (JointIndices.right_arm_joint, JointIndices.right_forearm_joint, HumanBodyBones.RightUpperArm, HumanBodyBones.RightShoulder),
        (JointIndices.right_forearm_joint, JointIndices.right_hand_joint, HumanBodyBones.RightLowerArm, HumanBodyBones.RightUpperArm),
        (JointIndices.right_hand_joint, JointIndices.right_handMidStart_joint, HumanBodyBones.RightHand, HumanBodyBones.RightLowerArm),
        (JointIndices.right_handThumbStart_joint, JointIndices.right_handThumb_1_joint, HumanBodyBones.RightThumbProximal, HumanBodyBones.RightHand),
        (JointIndices.right_handThumb_1_joint, JointIndices.right_handThumb_2_joint, HumanBodyBones.RightThumbIntermediate, HumanBodyBones.RightThumbProximal),
        (JointIndices.right_handThumb_2_joint, JointIndices.right_handThumbEnd_joint, HumanBodyBones.RightThumbDistal, HumanBodyBones.RightThumbIntermediate),
        (JointIndices.right_handIndexStart_joint, JointIndices.right_handIndex_1_joint, HumanBodyBones.RightIndexProximal, HumanBodyBones.RightHand),
        (JointIndices.right_handIndex_1_joint, JointIndices.right_handIndex_2_joint, HumanBodyBones.RightIndexIntermediate, HumanBodyBones.RightIndexProximal),
        (JointIndices.right_handIndex_2_joint, JointIndices.right_handIndex_3_joint, HumanBodyBones.RightIndexDistal, HumanBodyBones.RightIndexIntermediate),
        (JointIndices.right_handMidStart_joint, JointIndices.right_handMid_1_joint, HumanBodyBones.RightMiddleProximal, HumanBodyBones.RightHand),
        (JointIndices.right_handMid_1_joint, JointIndices.right_handMid_2_joint, HumanBodyBones.RightMiddleIntermediate, HumanBodyBones.RightMiddleProximal),
        (JointIndices.right_handMid_2_joint, JointIndices.right_handMid_3_joint, HumanBodyBones.RightMiddleDistal, HumanBodyBones.RightMiddleIntermediate),
        (JointIndices.right_handRingStart_joint, JointIndices.right_handRing_1_joint, HumanBodyBones.RightRingProximal, HumanBodyBones.RightHand),
        (JointIndices.right_handRing_1_joint, JointIndices.right_handRing_2_joint, HumanBodyBones.RightRingIntermediate, HumanBodyBones.RightRingProximal),
        (JointIndices.right_handRing_2_joint, JointIndices.right_handRing_3_joint, HumanBodyBones.RightRingDistal, HumanBodyBones.RightRingIntermediate),
        (JointIndices.right_handPinkyStart_joint, JointIndices.right_handPinky_1_joint, HumanBodyBones.RightLittleProximal, HumanBodyBones.RightHand),
        (JointIndices.right_handPinky_1_joint, JointIndices.right_handPinky_2_joint, HumanBodyBones.RightLittleIntermediate, HumanBodyBones.RightLittleProximal),
        (JointIndices.right_handPinky_2_joint, JointIndices.right_handPinky_3_joint, HumanBodyBones.RightLittleDistal, HumanBodyBones.RightLittleIntermediate),
        (JointIndices.left_shoulder_1_joint, JointIndices.left_arm_joint, HumanBodyBones.LeftShoulder, HumanBodyBones.UpperChest),
        (JointIndices.left_arm_joint, JointIndices.left_forearm_joint, HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftShoulder),
        (JointIndices.left_forearm_joint, JointIndices.left_hand_joint, HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftUpperArm),
        (JointIndices.left_hand_joint, JointIndices.left_handMidStart_joint, HumanBodyBones.LeftHand, HumanBodyBones.LeftLowerArm),
        (JointIndices.left_handThumbStart_joint, JointIndices.left_handThumb_1_joint, HumanBodyBones.LeftThumbProximal, HumanBodyBones.LeftHand),
        (JointIndices.left_handThumb_1_joint, JointIndices.left_handThumb_2_joint, HumanBodyBones.LeftThumbIntermediate, HumanBodyBones.LeftThumbProximal),
        (JointIndices.left_handThumb_2_joint, JointIndices.left_handThumbEnd_joint, HumanBodyBones.LeftThumbDistal, HumanBodyBones.LeftThumbIntermediate),
        (JointIndices.left_handIndexStart_joint, JointIndices.left_handIndex_1_joint, HumanBodyBones.LeftIndexProximal, HumanBodyBones.LeftHand),
        (JointIndices.left_handIndex_1_joint, JointIndices.left_handIndex_2_joint, HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftIndexProximal),
        (JointIndices.left_handIndex_2_joint, JointIndices.left_handIndex_3_joint, HumanBodyBones.LeftIndexDistal, HumanBodyBones.LeftIndexIntermediate),
        (JointIndices.left_handMidStart_joint, JointIndices.left_handMid_1_joint, HumanBodyBones.LeftMiddleProximal, HumanBodyBones.LeftHand),
        (JointIndices.left_handMid_1_joint, JointIndices.left_handMid_2_joint, HumanBodyBones.LeftMiddleIntermediate, HumanBodyBones.LeftMiddleProximal),
        (JointIndices.left_handMid_2_joint, JointIndices.left_handMid_3_joint, HumanBodyBones.LeftMiddleDistal, HumanBodyBones.LeftMiddleIntermediate),
        (JointIndices.left_handRingStart_joint, JointIndices.left_handRing_1_joint, HumanBodyBones.LeftRingProximal, HumanBodyBones.LeftHand),
        (JointIndices.left_handRing_1_joint, JointIndices.left_handRing_2_joint, HumanBodyBones.LeftRingIntermediate, HumanBodyBones.LeftRingProximal),
        (JointIndices.left_handRing_2_joint, JointIndices.left_handRing_3_joint, HumanBodyBones.LeftRingDistal, HumanBodyBones.LeftRingIntermediate),
        (JointIndices.left_handPinkyStart_joint, JointIndices.left_handPinky_1_joint, HumanBodyBones.LeftLittleProximal, HumanBodyBones.LeftHand),
        (JointIndices.left_handPinky_1_joint, JointIndices.left_handPinky_2_joint, HumanBodyBones.LeftLittleIntermediate, HumanBodyBones.LeftLittleProximal),
        (JointIndices.left_handPinky_2_joint, JointIndices.left_handPinky_3_joint, HumanBodyBones.LeftLittleDistal, HumanBodyBones.LeftLittleIntermediate),
        (JointIndices.neck_1_joint, JointIndices.head_joint, HumanBodyBones.Neck, HumanBodyBones.UpperChest),
        (JointIndices.head_joint, JointIndices.nose_joint, HumanBodyBones.Head, HumanBodyBones.Neck),
    };

    [SerializeField]
    [Tooltip("The ARHumanBodyManager which will produce frame events.")]
    ARHumanBodyManager m_HumanBodyManager;

    /// <summary>
    /// Get or set the <c>ARHumanBodyManager</c>.
    /// </summary>
    public ARHumanBodyManager humanBodyManager
    {
        get { return m_HumanBodyManager; }
        set { m_HumanBodyManager = value; }
    }

    [SerializeField]
    GameObject m_DestModelPrefab;

    Animator m_DestAnimator;
    Avatar m_SrcAvatar, m_DestAvatar;
    HumanPoseHandler m_SrcHandler, m_DestHandler;

    Dictionary<JointIndices, Transform> m_SrcBones = new Dictionary<JointIndices, Transform>();

    //Second argument is bind pose dirction. And third argument is child bone index
    Dictionary<JointIndices, (Transform, Vector3, JointIndices)> m_DestBones = 
        new Dictionary<JointIndices, (Transform, Vector3, JointIndices)>();

    bool m_Initialized = false;

    [SerializeField]
    float m_SrcScale = 0.05f;

    [SerializeField]
    float m_DestScale = 1.0f;

    void CreateSkeletonBone(string boneName, GameObject srcBone, 
                            List<SkeletonBone> skeletonBones, List<HumanBone> humanBones) 
    {
        var skeletonBone = new SkeletonBone();
        skeletonBone.name = boneName;
        skeletonBone.position = srcBone.transform.localPosition;
        skeletonBone.rotation = srcBone.transform.localRotation;
        skeletonBone.scale = srcBone.transform.localScale;
        skeletonBones.Add(skeletonBone);

        var humanBone = new HumanBone();
        humanBone.humanName = boneName;
        humanBone.boneName = boneName;
        humanBone.limit.useDefaultValues = true;
        humanBones.Add(humanBone);
    }

    GameObject CreateRootSrcBone() 
    {
        var rootSrcBone = new GameObject();
        rootSrcBone.name = "Root";
        rootSrcBone.transform.localPosition = Vector3.zero;
        rootSrcBone.transform.localRotation = Quaternion.identity;
        rootSrcBone.transform.localScale = Vector3.one;

        return rootSrcBone;
    }
    GameObject CreateSrcBone(JointIndices boneIndex, (string, string) boneName, 
                            GameObject rootSrcBone, 
                            Transform destTransform, Transform[] destComponents) 
    {
        var currentBoneName = boneName.Item1;
        var parentBoneName = boneName.Item2;

        var srcBone = new GameObject(currentBoneName);

        if((boneIndex == JointIndices.hips_joint))
        {
            srcBone.transform.SetParent(rootSrcBone.transform);

        } else {
            var rootComponents = rootSrcBone.GetComponentsInChildren<Transform>();
            foreach(var rootComponent in rootComponents) 
            {
                if(rootComponent.name != parentBoneName) { continue; } 
                srcBone.transform.SetParent(rootComponent.transform); 
                break; 
            }
        }

        var destName = destTransform.name;
        foreach(var destComponent in destComponents) 
        {
            if(destComponent.name != destName) { continue; }
            srcBone.transform.position = destComponent.transform.position;
            srcBone.transform.rotation = destComponent.transform.rotation;
            srcBone.transform.localScale = destComponent.transform.localScale;
            break; 
        }
        m_SrcBones[boneIndex] = srcBone.transform;

        return srcBone;
    }
    void CreateSrcBoneGizmo(GameObject srcBone) 
    {
        var scale = new Vector3(m_SrcScale, m_SrcScale, m_SrcScale);
        var cylinderScale = new Vector3(0.2f, 1.0f, 0.2f);

        var gizmo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gizmo.name = "Gizmo";
        gizmo.transform.SetParent(srcBone.transform);
        gizmo.transform.localPosition = Vector3.zero;
        gizmo.transform.localRotation = Quaternion.identity;
        gizmo.transform.localScale = scale;
        gizmo.GetComponent<Renderer>().material.color = Color.red; 

        var x = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        x.name = "GizmoX";
        x.transform.SetParent(gizmo.transform);
        x.transform.localPosition = Vector3.right;
        x.transform.localRotation = Quaternion.Euler(0, 0, 90);
        x.transform.localScale = cylinderScale;
        x.GetComponent<Renderer>().material.color = Color.red; 

        var y = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        y.name = "GizmoY";
        y.transform.SetParent(gizmo.transform);
        y.transform.localPosition = Vector3.up;
        y.transform.localRotation = Quaternion.Euler(0, 0, 0);
        y.transform.localScale = cylinderScale;
        y.GetComponent<Renderer>().material.color = Color.green; 

        var z = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        z.name = "GizmoZ";
        z.transform.SetParent(gizmo.transform);
        z.transform.localPosition = Vector3.forward;
        z.transform.localRotation = Quaternion.Euler(90, 0, 0);
        z.transform.localScale = cylinderScale;
        z.GetComponent<Renderer>().material.color = Color.blue; 
    }
    Dictionary<JointIndices, (string, string)> GetBoneNames(string[] humanBoneNames) 
    {
        var replaced = new Dictionary<string, string>();
        foreach(var humanBoneName in humanBoneNames) 
        { 
            var replacedBoneName = humanBoneName.Replace(" ", "");
            replaced[replacedBoneName] = humanBoneName;  
        }

        var boneNames = new Dictionary<JointIndices, (string, string)>();
        foreach(var boneMap in boneMaps) 
        { 
            var currentIndex = boneMap.Item1;
            var currentName = boneMap.Item3.ToString();
            var parentName = boneMap.Item4.ToString();

            boneNames[currentIndex] = (replaced[currentName], replaced[parentName]);
        }

        return boneNames;
    }
    bool BuildHumanAvatar(GameObject srcRootBone, List<SkeletonBone> skeletonBones, 
                            List<HumanBone> humanBones) 
    {
        var humanDescription = new HumanDescription();
        humanDescription.skeleton = skeletonBones.ToArray();
        humanDescription.human = humanBones.ToArray();

        m_SrcAvatar = AvatarBuilder.BuildHumanAvatar(srcRootBone, humanDescription);
        if (!m_SrcAvatar.isValid || !m_SrcAvatar.isHuman)
        {
            Debug.Log("avatar builder error");
            return false;
        }

        m_SrcHandler = new HumanPoseHandler(m_SrcAvatar, srcRootBone.transform);
        m_DestHandler = new HumanPoseHandler(m_DestAvatar, m_DestAnimator.transform);

        var humanPose = new HumanPose();
        m_SrcHandler.GetHumanPose(ref humanPose);
        m_DestHandler.SetHumanPose(ref humanPose);

        return true;
    }

    void OnEnable()
    {
        Debug.Assert(m_HumanBodyManager != null, "human body manager is required");
        m_HumanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;

        m_DestAnimator = m_DestModelPrefab.GetComponent<Animator>();
        m_DestAvatar = m_DestAnimator.avatar;
        var destComponents = m_DestModelPrefab.GetComponentsInChildren<Transform>();

        var skeletonBones = new List<SkeletonBone>();
        var humanBoneNames = HumanTrait.BoneName;
        var humanBones = new List<HumanBone>(humanBoneNames.Length);
        var boneNames = GetBoneNames(humanBoneNames);
        
        var rootSrcBone = CreateRootSrcBone();
        CreateSkeletonBone(rootSrcBone.name, rootSrcBone, skeletonBones, humanBones);

        var boneTransforms = new Dictionary<JointIndices, Transform>();
        foreach(var boneMap in boneMaps) 
        { 
            var boneIndex = boneMap.Item1; 
            var humanBodyBone = boneMap.Item3;
            boneTransforms[boneIndex] = m_DestAnimator.GetBoneTransform(humanBodyBone);
        }

        foreach(var boneMap in boneMaps) 
        { 
            var currentBoneIndex = boneMap.Item1;
            var childBoneIndex = boneMap.Item2;
            
            var boneName = boneNames[currentBoneIndex];
            var currentBoneName = boneName.Item1;
            
            var currentBoneTransform = boneTransforms[currentBoneIndex];

            var currentBonePos = currentBoneTransform.position;
            var childBonePos = currentBonePos;
            if(boneTransforms.ContainsKey(childBoneIndex)) {
                childBonePos = boneTransforms[childBoneIndex].position;
            }
            var poseDirection = (childBonePos - currentBonePos).normalized;

            var srcBone = CreateSrcBone(currentBoneIndex, boneName, rootSrcBone, 
                                            currentBoneTransform, destComponents);            
            CreateSrcBoneGizmo(srcBone);
            CreateSkeletonBone(currentBoneName, srcBone, skeletonBones, humanBones);

            m_DestBones[currentBoneIndex] = (currentBoneTransform, poseDirection, childBoneIndex);
        }

        if(BuildHumanAvatar(rootSrcBone, skeletonBones, humanBones)){ m_Initialized = true; }

        EnableUpdateWhenOffscreen();
    }

    private void Update()
    {
        if (!m_Initialized){ return; }
        if (m_SrcHandler == null || m_DestHandler == null) { return; }

        var humanPose = new HumanPose();
        m_SrcHandler.GetHumanPose(ref humanPose);
        m_DestHandler.SetHumanPose(ref humanPose);
    }

    void OnDisable()
    {
        Debug.Assert(m_HumanBodyManager != null, "human body manager is required");
        m_HumanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
    }

   Quaternion GetRotateY(ARHumanBody arBody, JointIndices rightBone, JointIndices leftBone)
   {
        var rightBonePosition = arBody.joints[(int)rightBone].anchorPose.position;
        var leftBonePosition = arBody.joints[(int)leftBone].anchorPose.position;
        var direction = leftBonePosition - rightBonePosition;
        direction.Set(direction.x, 0, direction.z);

        return Quaternion.FromToRotation(Vector3.left, direction);
    }
    (Quaternion, Quaternion, Quaternion) GetRotateY(ARHumanBody arBody) 
    {
        var lowerBodyRightBone = JointIndices.right_upLeg_joint;
        var lowerBodyLeftBone = JointIndices.left_upLeg_joint;
        var lowerBodyRotateY = GetRotateY(arBody, lowerBodyRightBone, lowerBodyLeftBone);

        var upperBodyRightBone = JointIndices.right_shoulder_1_joint;
        var upperBodyLeftBone = JointIndices.left_shoulder_1_joint;
        var upperBodyRotateY = GetRotateY(arBody, upperBodyRightBone, upperBodyLeftBone);

        var headRightBone = JointIndices.right_eye_joint;
        var headLeftBone = JointIndices.left_eye_joint;
        var headRotateY = GetRotateY(arBody, headRightBone, headLeftBone);

        return (lowerBodyRotateY, upperBodyRotateY, headRotateY);
    }
    Quaternion AddRotation(JointIndices curerntBoneIndex, Quaternion currentBoneRotation, 
                            (Quaternion, Quaternion, Quaternion) rotateY) 
    {
        var lowerBodyRotateY = rotateY.Item1;
        var upperBodyRotateY = rotateY.Item2;
        var headRotateY = rotateY.Item3;

        if(curerntBoneIndex == JointIndices.hips_joint || 
            (curerntBoneIndex >= JointIndices.spine_1_joint && curerntBoneIndex <= JointIndices.spine_3_joint))
        { 
            currentBoneRotation *= lowerBodyRotateY;
        }
        if(curerntBoneIndex >= JointIndices.spine_4_joint && curerntBoneIndex <= JointIndices.spine_7_joint) 
        { 
            currentBoneRotation *= upperBodyRotateY;
        }
        if((curerntBoneIndex >= JointIndices.neck_1_joint && curerntBoneIndex <= JointIndices.neck_4_joint) ||
            curerntBoneIndex == JointIndices.head_joint) 
        { 
            currentBoneRotation *= headRotateY;
        }

        return currentBoneRotation;
    }
    Vector3 AddPosition(Vector3 currentBonePosition) 
    {
        currentBonePosition += new Vector3(m_PosX, m_PosY, m_PosZ);
        return currentBonePosition;
    }

    void CreateOrUpdate(ARHumanBody arBody)
    {
        if (m_DestModelPrefab == null)
        {
            Debug.Log("no prefab found");
            return;
        }

        Transform rootTransform = arBody.transform;
        if (rootTransform == null)
        {
            Debug.Log("no root transform found for ARHumanBody");
            return;
        }
        
        var rotateY = GetRotateY(arBody);

        foreach(var destBone in m_DestBones) 
        {
            var curerntBoneIndex = destBone.Key;
            var childBoneIndex = destBone.Value.Item3;

            var currentBone = arBody.joints[(int)curerntBoneIndex];
            var childBone = arBody.joints[(int)childBoneIndex];

            var currentBonePosition = currentBone.anchorPose.position;
            var childBonePosition = childBone.anchorPose.position;
            
            var bindPoseDirection = destBone.Value.Item2;
            var poseDirection = (childBonePosition - currentBonePosition).normalized;
            var currentBoneRotation = Quaternion.FromToRotation(bindPoseDirection, poseDirection);

            currentBonePosition = AddPosition(currentBonePosition);
            currentBoneRotation = AddRotation(curerntBoneIndex, currentBoneRotation, rotateY);

            var srcBoneTransform = m_SrcBones[curerntBoneIndex].transform;
            srcBoneTransform.position = currentBonePosition * m_DestScale;
            srcBoneTransform.rotation = currentBoneRotation;
        }
    }

    void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("OnHumanBodiesChanged\n");

        sb.AppendFormat("   added[{0}]:\n", eventArgs.added.Count);
        foreach (var humanBody in eventArgs.added)
        {
            sb.AppendFormat("      human body: {0}\n", humanBody.ToString());
            CreateOrUpdate(humanBody);
        }

        sb.AppendFormat("   updated[{0}]:\n", eventArgs.updated.Count);
        foreach (var humanBody in eventArgs.updated)
        {
            sb.AppendFormat("      human body: {0}\n", humanBody.ToString());
            CreateOrUpdate(humanBody);
        }

        sb.AppendFormat("   removed[{0}]:\n", eventArgs.removed.Count);
        foreach (var humanBody in eventArgs.removed)
        {
            sb.AppendFormat("      human body: {0}\n", humanBody.ToString());
        }

        Debug.Log(sb.ToString());
    }

    float m_PosX = 0.0f, m_PosY = 0.0f, m_PosZ = 5.0f;
    private void OnGUI()
    {
        GUI.color = Color.white;
        GUI.skin.label.fontSize = 30;
        GUILayout.BeginVertical("box");

        m_PosX = GUI.HorizontalSlider(new Rect(50, 200, 500, 100), m_PosX, -10.0f, 10.0f);
        m_PosY = GUI.HorizontalSlider(new Rect(50, 300, 500, 100), m_PosY, -10.0f, 10.0f);
        m_PosZ = GUI.HorizontalSlider(new Rect(50, 400, 500, 100), m_PosZ, -10.0f, 10.0f);
        string x = m_PosX.ToString("F1"), y = m_PosY.ToString("F1"), z = m_PosZ.ToString("F1");
        GUI.Label(new Rect(50, 500, 500, 40), "PosX:" + x + ", PosY:" + y + ", PosZ:" + z);

        GUILayout.EndVertical();
    }
    private void EnableUpdateWhenOffscreen(){
        foreach (GameObject obj in FindObjectsOfType(typeof(GameObject))){
            if (!obj.activeInHierarchy) { continue; }

            var skinnedMeshRenderers = obj.GetComponents<SkinnedMeshRenderer>();
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers){
                skinnedMeshRenderer.updateWhenOffscreen = true;
            }
        }
    }

}
