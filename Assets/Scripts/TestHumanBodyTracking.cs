using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Simple script to spawn and render spheres at each joint position.
/// </summary>
public class TestHumanBodyTracking : MonoBehaviour
{
    /* 3D joint skeleton
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
    */

    [SerializeField]
    [Tooltip("The ARHumanBodyManager which will produce frame events.")]
    ARHumanBodyManager m_HumanBodyManager;

    [SerializeField]
    [Tooltip("Log all Human Body Changed events.")]
    bool m_VerboseLogging;

    [SerializeField]
    [Tooltip("UI to show current number of bodies being tracked.")]
    Text m_TextUI;

    Transform[] m_JointTransforms = new Transform[93];
    Transform m_RootTransform;
    int m_CurrentBodiesTracked;

    /// <summary>
    /// Get or set the <c>ARHumanBodyManager</c>.
    /// </summary>
    public ARHumanBodyManager humanBodyManager
    {
        get { return m_HumanBodyManager; }
        set
        {
            Debug.Assert(value != null, "Human Body Manager is required");
            
            if (value == m_HumanBodyManager)
                return;
            
            m_HumanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
            
            m_HumanBodyManager = value;
            
            m_HumanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
        }
    }

    public bool verboseLogging
    {
        get { return m_VerboseLogging; }
        set { m_VerboseLogging = value; }
    }

    public Text textUI
    {
        get { return m_TextUI; }
        set { m_TextUI = value; }
    }

    void OnEnable()
    {
        Debug.Assert(humanBodyManager != null, "human body manager is required");
        m_HumanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
    }

    void Start()
    {
        CreateRig();
    }

    void OnDisable()
    {
        Debug.Assert(humanBodyManager != null, "human body manager is required");
        m_HumanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
    }

    void CreateRig()
    {
        for (var i = 0; i < m_JointTransforms.Length; i++)
        {
            m_JointTransforms[i] = new GameObject().transform;
            var childTransform = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            childTransform.SetParent(m_JointTransforms[i]);
            childTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    void AssignRigTransforms(ARHumanBody arBody)
    {
        m_RootTransform = arBody.transform;
        if (m_RootTransform == null)
        {
            Debug.Log("no root transform found for ARHumanBody");
            return;
        }

        if (m_RootTransform.childCount <= 1)
        {
            for (var i = 1; i < arBody.joints.Length; i++)
            {
                m_JointTransforms[i].SetParent(m_RootTransform);
            }
        }
        else
        {
            m_JointTransforms[1].SetParent(m_RootTransform);
        }
    }

    void UpdateRig(ARHumanBody arBody)
    {
        for (var i = 1; i < arBody.joints.Length; i++)
        {
            var joint = arBody.joints[i];
            m_JointTransforms[i].localScale = joint.anchorScale;
            m_JointTransforms[i].localRotation = joint.anchorPose.rotation;
            m_JointTransforms[i].localPosition = joint.anchorPose.position;
        }
    }

    void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
    {
        if (verboseLogging)
            LogHumanBodiesChanged(eventArgs);
        
        foreach (var humanBody in eventArgs.added)
        {
            AssignRigTransforms(humanBody);
            m_CurrentBodiesTracked++;
        }

        foreach (var humanBody in eventArgs.updated)
        {
            UpdateRig(humanBody);
        }
        
        foreach (var humanBody in eventArgs.removed)
        {
            m_CurrentBodiesTracked--;
        }
        
        var sb = new StringBuilder();
        sb.AppendFormat("Number of bodies being tracked: {0}", m_CurrentBodiesTracked);
        textUI.text = sb.ToString();
    }

    static void LogHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
    {
        var sb = new StringBuilder();
        
        sb.Append("OnHumanBodiesChanged\n");
        sb.AppendFormat("   added[{0}]:\n", eventArgs.added.Count);
        foreach (var humanBody in eventArgs.added)
        {
            sb.AppendFormat("      human body: {0}\n", humanBody.ToString());
        }

        sb.AppendFormat("   updated[{0}]:\n", eventArgs.updated.Count);
        foreach (var humanBody in eventArgs.updated)
        {
            sb.AppendFormat("      human body: {0}\n", humanBody.ToString());
        }

        sb.AppendFormat("   removed[{0}]:\n", eventArgs.removed.Count);
        foreach (var humanBody in eventArgs.removed)
        {
            sb.AppendFormat("      human body: {0}\n", humanBody.ToString());
        }

        Debug.Log(sb.ToString());
    }
}
