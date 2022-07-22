/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Août 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Classe qui correspond aux éléments présents dans un message Pose publié par ROS.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PosRot
{
    public Vector3 position;
    public Quaternion orientation;
}
