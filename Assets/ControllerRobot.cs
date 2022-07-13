using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRobot : MonoBehaviour
{
    public bool Up;
    public bool Down;
    public bool Right;
    public bool Left;
    
    public void MoveUp()
    {
        Up = true;
        Down = false;
        Debug.Log("Up");
    }

    public void StopMove()
    {
        Up = false;
        Down = false;
    }

    public void MoveDown()
    {
        Down = true;
        Up = false;
        Debug.Log("Down");
    }
    
    public void ChangeJointR()
    {
        Right = true;
        Up = false;
        Down = false;
        Debug.Log("Right");
    }

    public void ChangeJointL()
    {
        Left = true;
        Up = false;
        Down = false;
        Debug.Log("Left");
    }
}
