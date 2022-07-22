using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidationTrajectoire : MonoBehaviour
{
    // Le script contenant les booléens pour l'état de la trajectoire
    public RobotVirtuel robot_virtuel;

    // Les boutons qui valident ou non la trajectoire
    private GameObject button_valider_trajectoire;
    private GameObject button_premier_point;

    // Le booléen qui signifie que le premier point est sélectionné
    public bool SetPremierPoint = false;

    void Start()
    {
        button_valider_trajectoire = GameObject.Find("Bouton valider trajectoire");
        button_premier_point = GameObject.Find("Bouton premier point");
    }

    public void ValiderPremierPoint()
    {
        SetPremierPoint = true;
        robot_virtuel.TrajectoireFinie = false;
        robot_virtuel.TrajectoireEnCours = false;
        robot_virtuel.point = new List<JointTrajectoryPoint>();
        robot_virtuel.trajectoire.points = null;
        robot_virtuel.triedre_robot_virtuel.GetComponent<Collider>().enabled = true;
        button_valider_trajectoire.SetActive(true);
        button_premier_point.SetActive(false);
    }

    public void ValiderTraj()
    {
        if(robot_virtuel.TrajectoireFinie == false)
        {
            robot_virtuel.TrajectoireFinie = true;
            robot_virtuel.trajectoire.points = robot_virtuel.point.ToArray();
            robot_virtuel.triedre_robot_virtuel.GetComponent<Collider>().enabled = false;
            button_valider_trajectoire.SetActive(false);
        }
    }

    public void FinTrajectoire()
    {
        robot_virtuel.TrajectoireFinie = false;
        robot_virtuel.TrajectoireEnCours = false;
        robot_virtuel.trajectoire.points = null;
        robot_virtuel.point = new List<JointTrajectoryPoint>();
        robot_virtuel.triedre_robot_virtuel.GetComponent<Collider>().enabled = true;
        button_premier_point.SetActive(true);
    }
}
