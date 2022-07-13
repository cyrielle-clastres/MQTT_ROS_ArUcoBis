using System;
using Unity.Robotics.UrdfImporter;
using UnityEngine;

public class SourceDestinationPublisher : MonoBehaviour
{
    const int k_NumRobotJoints = 8;

    public static readonly string[] LinkNames =
        { "base_link/base_link_inertia/shoulder_link", "/upper_arm_link", "/forearm_link", "/wrist_1_link",  "/wrist_2_link",  "/wrist_3_link", "/flange", "/tool0" };

    // Variables required for ROS communication
    [SerializeField]
    string m_TopicName = "/ur3e_joints";

    [SerializeField]
    GameObject ur3e;
    [SerializeField]
    GameObject m_Target;
    //[SerializeField]
    //GameObject m_TargetPlacement;
    //readonly Quaternion m_PickOrientation = Quaternion.Euler(90, 90, 0);

    // Robot Joints
    UrdfJointRevolute[] m_JointArticulationBodies;

    void Start()
    {
        m_JointArticulationBodies = new UrdfJointRevolute[k_NumRobotJoints];

        var linkName = string.Empty;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            linkName += LinkNames[i];
            m_JointArticulationBodies[i] = ur3e.transform.Find(linkName).GetComponent<UrdfJointRevolute>();
        }
    }

    public void Publish()
    {
        /*var sourceDestinationMessage;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            sourceDestinationMessage.joints[i] = m_JointArticulationBodies[i].GetPosition();
        }

        // Pick Pose
        sourceDestinationMessage.pick_pose = new PoseMsg
        {
            position = new Vector3(m_Target.transform.position.x, m_Target.transform.position.z, m_Target.transform.position.y),
            orientation = new Quaternion(-m_Target.transform.rotation.x, -m_Target.transform.rotation.z, -m_Target.transform.rotation.y, m_Target.transform.rotation.w)
        };

        // Place Pose
        /*sourceDestinationMessage.place_pose = new PoseMsg
        {
            position = m_TargetPlacement.transform.position.To<FLU>(),
            orientation = m_PickOrientation.To<FLU>()
        };*/
    }
}