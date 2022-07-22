/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Ao�t 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Ce script � pour but de d�placer le robot virtuel selon le d�placement du robot r�el.
 */

using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class RobotReel : MonoBehaviour
{
    // Nombre de liaisons du robot
    const int k_NumRobotJoints = 6;

    // Noms des liaisons du robot comme �crits dans Unity
    public static readonly string[] LinkNames =
        { "base_link/base_link_inertia/shoulder_link", "/upper_arm_link", "/forearm_link", "/wrist_1_link",  "/wrist_2_link",  "/wrist_3_link"};

    // Le robot
    [SerializeField]
    public GameObject ur3e;
    public GameObject ur3e_robot { get => ur3e; set => ur3e = value; }

    // Les articulations du robot
    public ArticulationBody[] m_JointArticulationBodies;

    // L'articulation first qui correspond � la base qui peut se d�placer dans l'espace.
    public ArticulationBody first;

    /*
     * Start est appel�e une seule fois au d�but/au lancement.
     * Ici, sont initialis�es les articulations du robot avec leur nom.
     */
    void Start()
    {
        // On cr�e 6 articulations
        m_JointArticulationBodies = new ArticulationBody[k_NumRobotJoints];

        var linkName = string.Empty;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            linkName += LinkNames[i];
            // On cherche l'articulation et on l'ajoute � la cha�ne.
            m_JointArticulationBodies[i] = ur3e.transform.Find(linkName).GetComponent<ArticulationBody>();
        }

        // On cherche la premi�re articulation pour pouvoir ensuite d�placer la base du robot dans l'espace.
        first = ur3e.transform.Find("base_link/base_link_inertia").GetComponent<ArticulationBody>();
    }

    /*
     * UpdatePosition est appel�e � chaque fois que le casque d�code un message sur le topic "position_robot".
     * La fonction permet d'attribuer aux 6 joints du robot virtuel, les valeurs des joints du robot r�el.
     * ATTENTION : Les articulations 0 et 2 sont invers�es de Unity au Robot r�el.
     */
    public void UpdatePosition(float[] position)
    {
        // On attribue au joint 2 sa position.
        var joint1XDrive = m_JointArticulationBodies[2].xDrive;
        joint1XDrive.target = (float)position[0] * Mathf.Rad2Deg;
        m_JointArticulationBodies[2].xDrive = joint1XDrive;

        // On attribue au joint 1 sa position.
        var joint2XDrive = m_JointArticulationBodies[1].xDrive;
        joint2XDrive.target = (float)position[1] * Mathf.Rad2Deg;
        m_JointArticulationBodies[1].xDrive = joint2XDrive;

        // On attribue au joint 0 sa position.
        var joint3XDrive = m_JointArticulationBodies[0].xDrive;
        joint3XDrive.target = (float)position[2] * Mathf.Rad2Deg;
        m_JointArticulationBodies[0].xDrive = joint3XDrive;

        // On attribue au joint 3 sa position.
        var joint4XDrive = m_JointArticulationBodies[3].xDrive;
        joint4XDrive.target = (float)position[3] * Mathf.Rad2Deg;
        m_JointArticulationBodies[3].xDrive = joint4XDrive;

        // On attribue au joint 4 sa position.
        var joint5XDrive = m_JointArticulationBodies[4].xDrive;
        joint5XDrive.target = (float)position[4] * Mathf.Rad2Deg;
        m_JointArticulationBodies[4].xDrive = joint5XDrive;

        // On attribue au joint 5 sa position.
        var joint6XDrive = m_JointArticulationBodies[5].xDrive;
        joint6XDrive.target = (float)position[5] * Mathf.Rad2Deg;
        m_JointArticulationBodies[5].xDrive = joint6XDrive;
    }
}