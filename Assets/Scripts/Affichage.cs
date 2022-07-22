/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Ao�t 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Ce script est appel� apr�s tous les autres scripts au d�marrage de l'application puisqu'il d�sactive certains objets.
 * Ce script g�re l'affichage ou non des objets dans l'environnement via la variable fixe, d�clar�e dans le script PlacementCube.
 * Il g�re cet affichage par plusieurs moyens : le toucher du tri�dre plac� au centre des ArUcos, le toucher des boutons qui permettent
 * de fixer et d�fixer les tri�dres.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class Affichage : MonoBehaviour , IMixedRealityTouchHandler
{
    // Le script contenant la variable fixe.
    public PlacementCube placement_cube;

    // Le script contenant les articulations du robot virtuel
    public RobotVirtuel robot_virtuel;

    // Objet qui g�re le placement des ArUcos
    private GameObject aruco_controller;

    // Les boutons qui permettent de bouger le tri�dre de la table
    protected GameObject buttons_translation;

    // Les boutons qui g�rent l'envoi de trajectoire
    protected GameObject button_valider_trajectoire;
    protected GameObject button_premier_point;

    // Les diff�rents tri�dres de notre espace
    private GameObject triedre_table_transrot;
    private GameObject triedre_table_calib;
    private GameObject triedre_effecteur;
    private GameObject triedre_robot;

    // Le tri�dre qui est associ� au robot virtuel. Il permet de faire bouger le robot virtuel.
    private GameObject triedre_robot_virtuel;

    // Les robots
    private GameObject ur3e;
    private GameObject ur3e_deplacement_virtuel;

    /* 
     * OnTouchStarted est appel�e � chaque fois qu'on touche le tri�dre central des ArUcos.
     * Cette fonction permet, si cela n'est pas d�j� fait, de d�sactiver la d�tection des ArUcos, d'afficher les boutons pour controller
     * le tri�dre en translation de la table, ainsi que les tri�dres associ�s.
     * Elle permet �galement de placer le tri�dre de la table qui va bouger � la m�me place que le tri�dre positionn�e par la calibration.
     */
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {   
        // Si les tri�dres sont en mouvement
        if (placement_cube.fixe == 0)
        {
            // On stoppe les tri�dres et affiche les nouveaux tri�dres de la table.
            placement_cube.fixe = 1;
            aruco_controller.SetActive(false);
            buttons_translation.SetActive(true);
            triedre_table_calib.SetActive(true);
            triedre_table_transrot.SetActive(true);

            // On positionne le tri�dre de la table qui va bouger au m�me endroit que le tri�dre issu de la calibration.
            triedre_table_transrot.transform.position = triedre_table_calib.transform.position;
            triedre_table_transrot.transform.rotation = triedre_table_calib.transform.rotation;

            // On permet le d�placement du robot virtuel et de son tri�dre en bougeant le robot r�el
            robot_virtuel.SetTriedre = false;
            robot_virtuel.SetJoints = false;
        }
    }

    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }

    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

    /*
     * NotAnchor est appel�e lorsque le bouton pour d�fixer les tri�dres est appuy�.
     * Cette fonction permet de remettre � 0 la calibration, en effa�ant les tri�dres de la table, du robot et de l'effecteur ainsi que les robots
     * et les boutons associ�s. Cette fonction permet donc de revenir � l'�tat initial du programme.
     * La d�tection des ArUcos est de nouveau possible.
     */
    public void NotAnchor()
    {
        // On active � nouveau la d�tection des ArUcos.
        placement_cube.fixe = 0;
        aruco_controller.SetActive(true);

        // On efface les boutons et les tri�dres
        buttons_translation.SetActive(false);
        button_valider_trajectoire.SetActive(false);
        button_premier_point.SetActive(false);
        triedre_table_calib.SetActive(false);
        triedre_table_transrot.SetActive(false);
        triedre_effecteur.SetActive(false);
        triedre_robot.SetActive(false);

        // On remet � 0 la matrice de transformation homog�ne entre le rep�re de la table et le rep�re de la table fait avec la calibration
        placement_cube.mat_table_tablecalib = Matrix4x4.identity;

        // On efface les robots et le tri�dre associ� au robot virtuel
        ur3e.SetActive(false);
        ur3e_deplacement_virtuel.SetActive(false);
        triedre_robot_virtuel.SetActive(false);

        // On permet le d�placement du robot virtuel et de son tri�dre en bougeant le robot r�el
        robot_virtuel.SetTriedre = false;
        robot_virtuel.SetJoints = false;
    }

    /*
     * Anchor est appel�e lorque le bouton pour arr�ter les translations est appuy�.
     * Cette fonction permet d'effacer les bouttons servant � la translation et le rep�re de la table issu de la calibration.
     * Elle permet d'afficher les robots, le tri�dre associ� au robot virtuel et �galement le tri�dre de l'effecteur et du robot r�el.
     */
    public void Anchor()
    {   
        // Si la d�tection des ArUcos n'est plus possible
        if (placement_cube.fixe == 1)
        {
            // On arr�te les translations
            placement_cube.fixe = 2;
            buttons_translation.SetActive(false);
            triedre_table_calib.SetActive(false);

            // On affiche les trid�res de l'effecteur et de la base du robot
            triedre_effecteur.SetActive(true);
            triedre_robot.SetActive(true);

            // On affiche les 2 robots et le tri�dre associ� au robot virtuel
            ur3e.SetActive(true);
            ur3e_deplacement_virtuel.SetActive(true);
            triedre_robot_virtuel.SetActive(true);

            // On affiche les boutons qui g�rent la trajectoire
            button_premier_point.SetActive(true);
        }
    }

    /*
     * Start est appel�e une seule fois au d�but/au lancement.
     * Ici est initialis� la recherche des objets suivants : les tri�dres, les boutons et les robots.
     * Ici est aussi g�r� l'affichage ou non des objets au d�marrage de l'application.
     */
    public void Start()
    {
        // On cherche tous les objets n�cessaires : tri�dres, robots et boutons.
        aruco_controller = GameObject.Find("ARUWP Controller");
        buttons_translation = GameObject.Find("Boutons translation");
        triedre_table_transrot = GameObject.Find("Triedre table transrot");
        triedre_table_calib = GameObject.Find("Triedre table");
        triedre_effecteur = GameObject.Find("Triedre effecteur");
        triedre_robot = GameObject.Find("Triedre Robot");
        ur3e = GameObject.Find("ur3e_robot");
        ur3e_deplacement_virtuel = GameObject.Find("ur3e_robot_unity2ROS");
        triedre_robot_virtuel = GameObject.Find("Triedre robot virtuel");
        button_valider_trajectoire = GameObject.Find("Bouton valider trajectoire");
        button_premier_point = GameObject.Find("Bouton premier point");

        // On d�sactive les objets non n�cessaires au d�but de l'application :
        // - les boutons
        if (buttons_translation != null)
        {
            buttons_translation.SetActive(false);
        }

        if(button_valider_trajectoire != null)
        {
            button_valider_trajectoire.SetActive(false);
        }

        if (button_premier_point != null)
        {
            button_premier_point.SetActive(false);
        }

        // - les tri�dres
        if (triedre_table_transrot != null)
        {
            triedre_table_transrot.SetActive(false);
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

        // -les robots
        if (ur3e != null)
        {
            ur3e.SetActive(false);
        }

        if (ur3e_deplacement_virtuel != null)
        {
            ur3e_deplacement_virtuel.SetActive(false);
        }

        // -le tri�dre associ� au robot virtuel
        if (triedre_robot_virtuel != null)
        {
            triedre_robot_virtuel.SetActive(false);
        }
    }
}