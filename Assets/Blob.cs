using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Blob : MonoBehaviour
{
    // Nombre de liaisons
    const int k_NumRobotJoints = 6;

    // Noms des liaisons
    public static readonly string[] LinkNames =
        { "base_link/base_link_inertia/shoulder_link", "/upper_arm_link", "/forearm_link", "/wrist_1_link",  "/wrist_2_link",  "/wrist_3_link"};

    [SerializeField]
    public GameObject ur3e;
    public GameObject ur3e_robot { get => ur3e; set => ur3e = value; }

    // Articulation Bodies
    public ArticulationBody[] m_JointArticulationBodies;
    public ArticulationBody first;

    /// <summary>
    ///     Find all robot joints in Awake() and add them to the jointArticulationBodies array.
    /// </summary>
    void Start()
    {
        m_JointArticulationBodies = new ArticulationBody[k_NumRobotJoints];

        var linkName = string.Empty;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            linkName += LinkNames[i];
            m_JointArticulationBodies[i] = ur3e.transform.Find(linkName).GetComponent<ArticulationBody>();
        }
        first = ur3e.transform.Find("base_link/base_link_inertia").GetComponent<ArticulationBody>();
    }
    
    public void UpdatePosition(float[] position)
    {
        //ATTENTION Les articulations 0 et 2 sont inversées de Unity au Robot !!!
        var joint1XDrive = m_JointArticulationBodies[2].xDrive;
        joint1XDrive.target = (float)position[0] * Mathf.Rad2Deg;
        m_JointArticulationBodies[2].xDrive = joint1XDrive;

        var joint2XDrive = m_JointArticulationBodies[1].xDrive;
        joint2XDrive.target = (float)position[1] * Mathf.Rad2Deg;
        m_JointArticulationBodies[1].xDrive = joint2XDrive;

        var joint3XDrive = m_JointArticulationBodies[0].xDrive;
        joint3XDrive.target = (float)position[2] * Mathf.Rad2Deg;
        m_JointArticulationBodies[0].xDrive = joint3XDrive;

        var joint4XDrive = m_JointArticulationBodies[3].xDrive;
        joint4XDrive.target = (float)position[3] * Mathf.Rad2Deg;
        m_JointArticulationBodies[3].xDrive = joint4XDrive;

        var joint5XDrive = m_JointArticulationBodies[4].xDrive;
        joint5XDrive.target = (float)position[4] * Mathf.Rad2Deg;
        m_JointArticulationBodies[4].xDrive = joint5XDrive;

        var joint6XDrive = m_JointArticulationBodies[5].xDrive;
        joint6XDrive.target = (float)position[5] * Mathf.Rad2Deg;
        m_JointArticulationBodies[5].xDrive = joint6XDrive;
    }
}