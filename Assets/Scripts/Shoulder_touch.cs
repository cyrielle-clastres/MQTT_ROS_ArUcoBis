using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class Shoulder_touch : MonoBehaviour, IMixedRealityTouchHandler
{
    public RobotController script;
    public ControllerRobot scriptControl;

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        scriptControl.Up = false;
        scriptControl.Down = false;
        script.previousIndex = script.selectedIndex;
        script.selectedIndex = 2;
        script.Highlight(script.selectedIndex);
    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }
}
