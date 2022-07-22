/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Ao�t 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Classe qui correspond aux �l�ments pr�sents dans un message Pose publi� par ROS.
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
