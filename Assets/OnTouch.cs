using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class OnTouch : MonoBehaviour , IMixedRealityTouchHandler
{
    public PlacementCube script;
    private GameObject ArUco; //Objet qui gère le placement des ArUcos

    //Les boutons qui permettent de bouger le trièdre de la table
    protected GameObject six_buttons;
    protected GameObject button_fixe;

    //Les différents trièdres de notre espace
    private GameObject triedre_table_transrot;
    private GameObject triedre_table_calib;
    private GameObject triedre_effecteur;
    private GameObject triedre_robot;
    private GameObject triedre_robot_virtuel;

    //Le robot
    private GameObject ur3e;
    private GameObject ur3e_deplacement_virtuel;

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {   if (script.fixe == 0)
        {
            script.fixe = 1;
            ArUco.SetActive(false);
            six_buttons.SetActive(true);
            button_fixe.SetActive(true);
            triedre_table_calib.SetActive(true);
            triedre_table_transrot.SetActive(true);
            triedre_table_transrot.transform.position = triedre_table_calib.transform.position;
            triedre_table_transrot.transform.rotation = triedre_table_calib.transform.rotation;
        }
    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

    public void NotAnchor()
    {
        script.fixe = 0;
        ArUco.SetActive(true);
        six_buttons.SetActive(false);
        button_fixe.SetActive(false);
        triedre_table_calib.SetActive(false);
        triedre_table_transrot.SetActive(false);
        triedre_effecteur.SetActive(false);
        triedre_robot.SetActive(false);
        script.mat_table_tablecalib = Matrix4x4.identity;
        ur3e.SetActive(false);
        ur3e_deplacement_virtuel.SetActive(false);
        triedre_robot_virtuel.SetActive(false);
    }

    public void Anchor()
    {
        if (script.fixe == 1)
        {
            script.fixe = 2;
            six_buttons.SetActive(false);
            button_fixe.SetActive(false);
            triedre_table_calib.SetActive(false);
            triedre_effecteur.SetActive(true);
            triedre_robot.SetActive(true);
            ur3e.SetActive(true);
            ur3e_deplacement_virtuel.SetActive(true);
            triedre_robot_virtuel.SetActive(true);
        }
    }

    public void Start()
    {
        ArUco = GameObject.Find("ARUWP Controller");
        six_buttons = GameObject.Find("Boutons translation");
        button_fixe = GameObject.Find("Stopper mouvement");
        triedre_table_transrot = GameObject.Find("Triedre table transrot");
        triedre_table_calib = GameObject.Find("Triedre table");
        triedre_effecteur = GameObject.Find("Triedre effecteur");
        triedre_robot = GameObject.Find("Triedre Robot");
        ur3e = GameObject.Find("ur3e_robot");
        ur3e_deplacement_virtuel = GameObject.Find("ur3e_robot_unity2ROS");
        triedre_robot_virtuel = GameObject.Find("Triedre robot virtuel");
        if (six_buttons != null)
        {
            six_buttons.SetActive(false);
        }
        if (triedre_table_transrot != null)
        {
            triedre_table_transrot.SetActive(false);
        }
        if (button_fixe != null)
        {
            button_fixe.SetActive(false);
        }
        if (triedre_effecteur != null)
        {
            triedre_effecteur.SetActive(false);
        }
        if (triedre_robot != null)
        {
            triedre_robot.SetActive(false);
        }
        if (triedre_table_transrot != null)
        {
            triedre_table_transrot.SetActive(false);
        }
        if (triedre_table_calib != null)
        {
            triedre_table_calib.SetActive(false);
        }
        if (ur3e != null)
        {
            ur3e.SetActive(false);
        }
        if (ur3e_deplacement_virtuel != null)
        {
            ur3e_deplacement_virtuel.SetActive(false);
        }
        if (triedre_robot_virtuel != null)
        {
            triedre_robot_virtuel.SetActive(false);
        }
    }
}