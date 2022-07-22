/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Août 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Classe qui correspond aux éléments présents dans un message JointTrajectoryPoint publié par ROS.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JointTrajectoryPoint
{
    public float[] positions;
    public float[] velocities;
    public float[] accelerations;
    public float[] effort;
    public float time_from_start;

    public JointTrajectoryPoint()
    {
        positions = null;
        velocities = null;
        accelerations = null;
        effort = null;
        time_from_start = 0;
    }
}

