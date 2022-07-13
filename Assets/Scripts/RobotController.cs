using UnityEngine;
using Unity.Robotics;
using Unity.Robotics.UrdfImporter.Control;
using UrdfControlRobot = Unity.Robotics.UrdfImporter.Control;
using UnityEngine.InputSystem;

public class RobotController : MonoBehaviour
{
    public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };
    public enum ControlType { PositionControl };

    public ControllerRobot script;

    //Robot properties
    private ArticulationBody[] articulationChain;
    private Color[] prevColor;
    [InspectorReadOnly(hideInEditMode: true)]
    public string selectedJoint;
    [HideInInspector]
    public int previousIndex;
    public int selectedIndex;
    [Header("Robot properties")]
    public ControlType control = ControlType.PositionControl;
    public float stiffness;
    public float damping;
    public float forceLimit;
    public float speed = 5f; // Units: degree/s
    public float torque = 100f; // Units: Nm or N
    public float acceleration = 5f;// Units: m/s^2 / degree/s^2
    [Tooltip("Color to highlight the currently selected Join")]
    public Color highLightColor = new Color(1, 0, 0, 1);

    //The old controller
    private Controller controller;

    public void MoveUp()
    {
        script.Up = true;
        Debug.Log("Up");
    }

    public void MoveDown()
    {
        script.Down = true;
    }

    public void ChangeJointR()
    {
        script.Right = true;
        Debug.Log("Right");
    }

    public void ChangeJointL()
    {
        script.Left = true;
    }

    private void OnEnable()
    {
        this.gameObject.AddComponent(typeof(Controller));
        controller = GetComponent<Controller>();
        SetControllerValues();
    }
    void Start()
    {
        previousIndex = selectedIndex = 1;
        this.gameObject.AddComponent<FKRobot>();
        articulationChain = this.GetComponentsInChildren<ArticulationBody>();
        int defDyanmicVal = 10;
        foreach (ArticulationBody joint in articulationChain)
        {
            joint.gameObject.AddComponent<JointControl>();
            joint.jointFriction = defDyanmicVal;
            joint.angularDamping = defDyanmicVal;
            ArticulationDrive currentDrive = joint.xDrive;
            currentDrive.forceLimit = forceLimit;
            joint.xDrive = currentDrive;
        }
        DisplaySelectedJoint(selectedIndex);
        StoreJointColors(selectedIndex);
    }
    private void Update()
    {
        bool SelectionInput1 = script.Right;
        bool SelectionInput2 = script.Left;

        SetSelectedJointIndex(selectedIndex); // to make sure it is in the valid range
        UpdateDirection(selectedIndex);

        if (SelectionInput2)
        {
            SetSelectedJointIndex(selectedIndex - 1);
            Highlight(selectedIndex);
        }
        else if (SelectionInput1)
        {
            SetSelectedJointIndex(selectedIndex + 1);
            Highlight(selectedIndex);
        }
        //Highlight(selectedIndex);
        Debug.Log("Selected index = " + selectedIndex);
        UpdateDirection(selectedIndex);
    }
    private void SetSelectedJointIndex(int index)
    {
        if (articulationChain.Length > 0)
        {
            selectedIndex = (index + articulationChain.Length) % articulationChain.Length;
        }
    }
    /// <summary>
    /// Highlights the color of the robot by changing the color of the part to a color set by the user in the inspector window
    /// </summary>
    /// <param name="selectedIndex">Index of the link selected in the Articulation Chain</param>
    public void Highlight(int selectedIndex)
    {
        if (selectedIndex == previousIndex || selectedIndex < 0 || selectedIndex >= articulationChain.Length)
        {
            return;
        }

        // reset colors for the previously selected joint
        ResetJointColors(previousIndex);

        // store colors for the current selected joint
        StoreJointColors(selectedIndex);

        DisplaySelectedJoint(selectedIndex);
        Renderer[] rendererList = articulationChain[selectedIndex].transform.GetChild(1).GetComponentsInChildren<Renderer>();

        // set the color of the selected join meshes to the highlight color
        foreach (var mesh in rendererList)
        {
            MaterialExtensions.SetMaterialColor(mesh.material, highLightColor);
        }
    }
    /// <summary>
    /// Sets the direction of movement of the joint on every update
    /// </summary>
    /// <param name="jointIndex">Index of the link selected in the Articulation Chain</param>
    private void UpdateDirection(int jointIndex)
    {
        if (jointIndex < 0 || jointIndex >= articulationChain.Length)
        {
            return;
        }
        float moveDirection;
        if (script.Up)
        {
            moveDirection = 1;
        }
        else if (script.Down)
        {
            moveDirection = -1;
        }
        else
        {
            moveDirection = 0;
        }

        JointControl current = articulationChain[jointIndex].GetComponent<JointControl>();
        if (previousIndex != jointIndex)
        {
            JointControl previous = articulationChain[previousIndex].GetComponent<JointControl>();
            previous.direction = UrdfControlRobot.RotationDirection.None;
            previousIndex = jointIndex;
        }

        if (current.controltype != UrdfControlRobot.ControlType.PositionControl)
        {
            UpdateControlType(current);
        }

        if (moveDirection > 0)
        {
            current.direction = UrdfControlRobot.RotationDirection.Positive;
        }
        else if (moveDirection < 0)
        {
            current.direction = UrdfControlRobot.RotationDirection.Negative;
        }
        else
        {
            current.direction = UrdfControlRobot.RotationDirection.None;
        }
        script.Right = false;
        script.Left = false;
    }
    /// <summary>
    /// Stores original color of the part being highlighted
    /// </summary>
    /// <param name="index">Index of the part in the Articulation chain</param>
    private void StoreJointColors(int index)
    {
        Renderer[] materialLists = articulationChain[index].transform.GetChild(1).GetComponentsInChildren<Renderer>();
        prevColor = new Color[materialLists.Length];
        for (int counter = 0; counter < materialLists.Length; counter++)
        {
            prevColor[counter] = MaterialExtensions.GetMaterialColor(materialLists[counter]);
        }
    }/// <summary>
    /// Resets original color of the part being highlighted
    /// </summary>
    /// <param name="index">Index of the part in the Articulation chain</param>
    private void ResetJointColors(int index)
    {
        Renderer[] previousRendererList = articulationChain[index].transform.GetChild(1).GetComponentsInChildren<Renderer>();
        for (int counter = 0; counter < previousRendererList.Length; counter++)
        {
            MaterialExtensions.SetMaterialColor(previousRendererList[counter].material, prevColor[counter]);
        }
    }
    void DisplaySelectedJoint(int selectedIndex)
    {
        if (selectedIndex < 0 || selectedIndex >= articulationChain.Length)
        {
            return;
        }
        selectedJoint = articulationChain[selectedIndex].name + " (" + selectedIndex + ")";
    }
    public void UpdateControlType(JointControl joint)
    {
        joint.controltype = UrdfControlRobot.ControlType.PositionControl;
        if (control == ControlType.PositionControl)
        {
            ArticulationDrive drive = joint.joint.xDrive;
            drive.stiffness = stiffness;
            drive.damping = damping;
            joint.joint.xDrive = drive;
        }
    }
    private void SetControllerValues()
    {
        controller.stiffness = stiffness;
        controller.damping = damping;
        controller.forceLimit = forceLimit;
        controller.speed = speed;
        controller.torque = torque;
        controller.acceleration = acceleration;
    }
    private void FixedUpdate()
    {
        SetControllerValues();
    }
}
