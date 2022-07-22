/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Ao�t 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Classe qui correspond aux �l�ments pr�sents dans un message JointState publi� par ROS.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JointState
{
    public string[] name;
    public float[] position;
    public float[] velocity;
    public float[] effort;

    public JointState()
    {
        name = new string[6];
        position = null;
        velocity = null;
        effort = null;
    }
}
