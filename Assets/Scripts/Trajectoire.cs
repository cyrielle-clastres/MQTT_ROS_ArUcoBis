using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trajectoire
{
    public string[] joint_names;
    public JointTrajectoryPoint[] points;

    public Trajectoire()
    {
        joint_names = null;
        points = null;
    }
}
