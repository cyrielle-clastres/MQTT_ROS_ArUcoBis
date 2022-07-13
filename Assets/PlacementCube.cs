using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class PlacementCube : MonoBehaviour
{
    //Les 3 directions qui vont permettre de tracer le repère de la table
    public Vector3 directionu1;
    public Vector3 directionu2;
    public Vector3 normal;

    //Permet de fixer le repère de la table
    public int fixe = 0;

    //Le trièdre des ArUco et de la table
    protected GameObject triedre;
    protected GameObject triedre_table;
    protected GameObject triedre_robot;

    //Les trièdres des ArUco
    protected GameObject cube0;
    protected GameObject cube3;
    protected GameObject cube8;
    protected GameObject cube11;

    //Le bouton qui permet de "défixer" le trièdre de la table
    protected GameObject button;
    //Les boutons qui permetend de bouger le trièdre de la table
    protected GameObject six_buttons;

    //Le robot
    [SerializeField]
    public GameObject ur3e;
    public GameObject ur3e_robot { get => ur3e; set => ur3e = value; }

    public GameObject ur3e_deplacement_virtuel;
    public GameObject ur3e_robot_deplacement_virtuel { get => ur3e_deplacement_virtuel; set => ur3e_deplacement_virtuel = value; }

    public Blob script_robot;
    public RobotVirtuel script_robot_virtuel;

    //La matrice de transformation homogène de la table vers le monde et du monde vers la table
    public Matrix4x4 mat_tablecalib_monde = Matrix4x4.identity;
    public Matrix4x4 mat_table_monde = Matrix4x4.identity;
    public Matrix4x4 mat_monde_table = Matrix4x4.identity;
    public Matrix4x4 mat_table_tablecalib = Matrix4x4.identity;
    protected Matrix4x4 mat_robot_table = Matrix4x4.identity;
    public Matrix4x4 mat_robot_monde = Matrix4x4.identity;
    public Matrix4x4 mat_monde_robot = Matrix4x4.identity;

    // Start is called before the first frame update
    void Start()
    {
        triedre = GameObject.Find("Triedre feuille");
        triedre_table = GameObject.Find("Triedre table");
        button = GameObject.Find("PressableButtonHoloLens2");
        triedre_robot = GameObject.Find("Triedre Robot");
        ur3e = GameObject.Find("ur3e_robot");
        ur3e_deplacement_virtuel = GameObject.Find("ur3e_robot_unity2ROS");
        mat_robot_table.SetColumn(0, new Vector4(0, 0, 1, 0));
        mat_robot_table.SetColumn(1, new Vector4(0, 1, 0, 0));
        mat_robot_table.SetColumn(2, new Vector4(-1, 0, 0, 0));
        mat_robot_table.SetColumn(3, new Vector4(0.259f, 0, 0.385f, 1));
    }

    // Update is called once per frame
    void Update()
    {
        cube0 = GameObject.Find("Cube00 Scene Root");
        cube3 = GameObject.Find("Cube03 Scene Root");
        cube8 = GameObject.Find("Cube08 Scene Root");
        cube11 = GameObject.Find("Cube11 Scene Root");
        six_buttons = GameObject.Find("Boutons translation");

        //Placement d'un cube au centre des 4 marqueurs détectés.
        if ((cube0 != null) && (cube3 != null) && (cube8 != null) && (cube11 != null))
        {
            Vector3 pos = new Vector3();
            Quaternion rot = new Quaternion();
            pos.x = (cube0.transform.position.x + cube3.transform.position.x + cube8.transform.position.x + cube11.transform.position.x) / 4;
            pos.y = (cube0.transform.position.y + cube3.transform.position.y + cube8.transform.position.y + cube11.transform.position.y) / 4;
            pos.z = (cube0.transform.position.z + cube8.transform.position.z + cube3.transform.position.z + cube11.transform.position.z) / 4;

            rot.x = (cube0.transform.rotation.x + cube3.transform.rotation.x + cube8.transform.rotation.x + cube11.transform.rotation.x) / 4;
            rot.y = (cube0.transform.rotation.y + cube3.transform.rotation.y + cube8.transform.rotation.y + cube11.transform.rotation.y) / 4;
            rot.z = (cube0.transform.rotation.z + cube3.transform.rotation.z + cube8.transform.rotation.z + cube11.transform.rotation.z) / 4;
            rot.w = (cube0.transform.rotation.w + cube3.transform.rotation.w + cube8.transform.rotation.w + cube11.transform.rotation.w) / 4;
            
            DrawLine(pos, rot);
        }
    }

    /*
     * Fonction qui permet de tracer le repère de la table en direct, le repère des ArUcos en indirect et qui calcule la transformation du repère
     * monde au repère de la table en indirect. Les 3 directions sont calculées ici. Est également calculé ici, la position du bouton permettant
     * de débloquer le trièdre de la table.
     */
    void DrawLine(Vector3 pos, Quaternion rot)
    {
        if (fixe == 0)
        {
            // Matrices de transformation
            Matrix4x4 mat_table_aruco = Matrix4x4.identity;
            Matrix4x4 mat_aruco_monde = Matrix4x4.identity;
            
            //Directionu1 = x et Directionu2 = y Normal = z car Repère Unity en indirect
            directionu1 = cube3.transform.position - cube0.transform.position;
            directionu2 = cube3.transform.position - cube11.transform.position;
            normal = Vector3.Cross(directionu1, directionu2);

            //Centre est ici le point de pose du trièdre correspondant à la table
            Vector3 centre = pos - directionu1.normalized * 1.0f - directionu2.normalized * 0.525f - normal.normalized * 0.63f;

            //On calcule la matrice de transformation homogène pour passer du repère de la table au repère des ArUcos exprimé dans le repère des ArUcos
            mat_table_aruco.SetColumn(0, new Vector4(1, 0, 0, 0));
            mat_table_aruco.SetColumn(1, new Vector4(0, 1, 0, 0));
            mat_table_aruco.SetColumn(2, new Vector4(0, 0, 1, 0));
            mat_table_aruco.SetColumn(3, new Vector4(centre.x - pos.x, centre.y - pos.y, centre.z - pos.z, 1));

            //On calcule la matrice de transformation homogène pour passer du repère des ArUcos au repère du monde
            mat_aruco_monde.SetColumn(0, new Vector4((directionu1.normalized).x, (directionu1.normalized).y, (directionu1.normalized).z, 0));
            mat_aruco_monde.SetColumn(1, new Vector4((directionu2.normalized).x, (directionu2.normalized).y, (directionu2.normalized).z, 0));
            mat_aruco_monde.SetColumn(2, new Vector4((normal.normalized).x, (normal.normalized).y, (normal.normalized).z, 0));
            mat_aruco_monde.SetColumn(3, new Vector4(pos.x, pos.y, pos.z, 1));

            //On calcule la matrice de transformation homogène pour passer du repère de la table au repère du monde
            mat_tablecalib_monde = mat_table_aruco * mat_aruco_monde;

            //On calcule la matrice de transformation homogène pour passer du repère du monde au repère de la table
            mat_monde_table = mat_tablecalib_monde.inverse;

            //Place le trièdre de la table
            triedre_table.transform.position = centre;
            triedre_table.transform.rotation = mat_aruco_monde.rotation;

            //Place le trièdre des ArUco
            triedre.transform.rotation = mat_aruco_monde.rotation;
            triedre.transform.position = pos - normal.normalized * 0.03f;

            //Placer le bouton pour défixer le repère dans l'espace
            float sqrLen = (pos + directionu1.normalized * 0.40f - directionu2.normalized * 0.55f - button.transform.position).sqrMagnitude;
            if (sqrLen > 0.5)
            {
                button.transform.position = pos + directionu1.normalized * 0.30f - directionu2.normalized * 0.1f - normal.normalized * 0.55f;
                Vector3 rot_button = rot.eulerAngles;
                rot_button = new Vector3(rot_button.x + 180, rot_button.y, rot_button.z);
                button.transform.rotation = Quaternion.Euler(rot_button);
            }
        }
        else if ((fixe == 1)&&(six_buttons != null))
        {
            six_buttons.transform.position = pos - directionu1.normalized * 1.0f - directionu2.normalized * 0.3f - normal.normalized * 0.63f;
            six_buttons.transform.rotation = mat_tablecalib_monde.rotation;
        }
        else if (fixe == 2)
        {
            mat_robot_monde = mat_table_monde * mat_robot_table;
            mat_monde_robot = mat_robot_monde.inverse;
            triedre_robot.transform.rotation = mat_robot_monde.rotation;
            triedre_robot.transform.position = new Vector3(mat_robot_monde[0,3] - directionu2.normalized.x * 0.02f, mat_robot_monde[1,3] - directionu2.normalized.y * 0.02f, mat_robot_monde[2,3] - directionu2.normalized.z * 0.02f);

            Vector3 rotation = mat_robot_monde.rotation.eulerAngles;
            rotation = new Vector3(rotation.x, rotation.y + 90, rotation.z);
            script_robot.first.TeleportRoot(new Vector3(mat_robot_monde[0, 3] + directionu1.normalized.x * 0.5f - directionu2.normalized.x * 0.02f, mat_robot_monde[1, 3] + directionu1.normalized.y * 0.5f - directionu2.normalized.y * 0.02f, mat_robot_monde[2, 3] + directionu1.normalized.z * 0.5f - directionu2.normalized.z * 0.02f), Quaternion.Euler(rotation));
            script_robot_virtuel.first.TeleportRoot(new Vector3(mat_robot_monde[0, 3], mat_robot_monde[1, 3], mat_robot_monde[2, 3]), Quaternion.Euler(rotation));
        }
    }
}
