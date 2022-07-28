/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Août 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Ce script est appelé après tous les autres scripts au démarrage de l'application puisqu'il désactive certains objets.
 * Ce script gère l'affichage ou non des objets dans l'environnement via la variable fixe, déclarée dans le script PlacementCube.
 * Il gère cet affichage par plusieurs moyens : le toucher du trièdre placé au centre des ArUcos, le toucher des boutons qui permettent
 * de fixer et défixer les trièdres.
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

    // Le script contenant les articulations du robot réel
    public RobotReel robot_reel;

    // Le script qui contient la trajectoire
    public ValidationTrajectoire validation_trajectoire;

    // Objet qui gère le placement des ArUcos
    private GameObject aruco_controller;

    // Les boutons qui permettent de bouger le trièdre de la table
    protected GameObject buttons_translation_rotation;

    // Les boutons qui gèrent l'envoi de trajectoire
    protected GameObject button_valider_trajectoire;
    protected GameObject button_premier_point;
    protected GameObject button_annuler_trajectoire;

    // Les différents trièdres de notre espace
    private GameObject triedre_table_transrot;
    private GameObject triedre_table_calib;
    private GameObject triedre_effecteur;
    private GameObject triedre_robot;

    // Le trièdre qui est associé au robot virtuel. Il permet de faire bouger le robot virtuel.
    private GameObject triedre_robot_virtuel;

    /* 
     * OnTouchStarted est appelée à chaque fois qu'on touche le trièdre central des ArUcos.
     * Cette fonction permet, si cela n'est pas déjà fait, de désactiver la détection des ArUcos, d'afficher les boutons pour controller
     * le trièdre en translation de la table, ainsi que les trièdres associés.
     * Elle permet également de placer le trièdre de la table qui va bouger à la même place que le trièdre positionnée par la calibration.
     */
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {   
        // Si les trièdres sont en mouvement
        if (placement_cube.fixe == 0)
        {
            // On stoppe les trièdres et affiche les nouveaux trièdres de la table.
            placement_cube.fixe = 1;
            aruco_controller.SetActive(false);
            buttons_translation_rotation.SetActive(true);
            triedre_table_calib.SetActive(true);
            triedre_table_transrot.SetActive(true);

            // On positionne le trièdre de la table qui va bouger au même endroit que le trièdre issu de la calibration.
            triedre_table_transrot.transform.position = triedre_table_calib.transform.position;
            triedre_table_transrot.transform.rotation = triedre_table_calib.transform.rotation;

            // On permet le déplacement du robot virtuel et de son trièdre en bougeant le robot réel
            robot_virtuel.SetTriedre = false;
            robot_virtuel.SetJoints = false;
        }
    }

    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }

    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

    /*
     * NotAnchor est appelée lorsque le bouton pour défixer les trièdres est appuyé.
     * Cette fonction permet de remettre à 0 la calibration, en effaçant les trièdres de la table, du robot et de l'effecteur ainsi que les robots
     * et les boutons associés. Cette fonction permet donc de revenir à l'état initial du programme.
     * La détection des ArUcos est de nouveau possible.
     */
    public void NotAnchor()
    {
        // On active à nouveau la détection des ArUcos.
        placement_cube.fixe = 0;
        aruco_controller.SetActive(true);

        // On efface les boutons et les trièdres
        buttons_translation_rotation.SetActive(false);
        button_valider_trajectoire.SetActive(false);
        button_premier_point.SetActive(false);
        button_annuler_trajectoire.SetActive(false);
        triedre_table_calib.SetActive(false);
        triedre_table_transrot.SetActive(false);
        triedre_effecteur.SetActive(false);
        triedre_robot.SetActive(false);

        // On remet à 0 la matrice de transformation homogène entre le repère de la table et le repère de la table fait avec la calibration
        placement_cube.mat_table_tablecalib = Matrix4x4.identity;
        
        // On efface la trajectoire qui a été tracée si jamais c'est le cas
        validation_trajectoire.AnnulerTrajectoire();

        // On ne permet plus le déplacement du robot virtuel et de son trièdre en bougeant le robot réel
        robot_virtuel.SetTriedre = false;
        robot_virtuel.SetJoints = false;

        // On efface les robots et le trièdre associé au robot virtuel
        robot_reel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
        robot_virtuel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
        triedre_robot_virtuel.SetActive(false);

        // On efface les boutons qui gèrent la trajectoire
        button_premier_point.SetActive(false);
    }

    /*
     * Anchor est appelée lorque le bouton pour arrêter les translations est appuyé.
     * Cette fonction permet d'effacer les bouttons servant à la translation et le repère de la table issu de la calibration.
     * Elle permet d'afficher les robots, le trièdre associé au robot virtuel et également le trièdre de l'effecteur et du robot réel.
     */
    public void Anchor()
    {   
        // Si la détection des ArUcos n'est plus possible
        if (placement_cube.fixe == 1)
        {
            // On arrête les translations
            placement_cube.fixe = 2;
            buttons_translation_rotation.SetActive(false);
            triedre_table_calib.SetActive(false);

            // On affiche les tridères de l'effecteur et de la base du robot
            triedre_effecteur.SetActive(true);
            triedre_robot.SetActive(true);

            // On affiche le trièdre associé au robot virtuel
            triedre_robot_virtuel.SetActive(true);

            // On affiche les boutons qui gèrent la trajectoire
            button_premier_point.SetActive(true);
        }
    }

    public void RetourCalibration()
    {
        // Si les trièdres sont en mouvement
        if (placement_cube.fixe == 2)
        {
            // On stoppe les trièdres et affiche les nouveaux trièdres de la table.
            placement_cube.fixe = 1;
            aruco_controller.SetActive(false);
            buttons_translation_rotation.SetActive(true);
            triedre_table_calib.SetActive(true);
            triedre_table_transrot.SetActive(true);

            // On positionne le trièdre de la table qui va bouger au même endroit que le trièdre issu de la calibration.
            triedre_table_transrot.transform.position = triedre_table_calib.transform.position;
            triedre_table_transrot.transform.rotation = triedre_table_calib.transform.rotation;

            // On permet le déplacement du robot virtuel et de son trièdre en bougeant le robot réel
            robot_virtuel.SetTriedre = false;
            robot_virtuel.SetJoints = false;

            // On efface le robot à côté
            robot_reel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));

            // On affiche les tridères de l'effecteur et de la base du robot
            triedre_effecteur.SetActive(false);
            triedre_robot.SetActive(false);

            // On affiche le trièdre associé au robot virtuel
            triedre_robot_virtuel.SetActive(false);

            // On affiche les boutons qui gèrent la trajectoire
            button_premier_point.SetActive(false);

            // On efface la trajectoire qui a été tracée si jamais c'est le cas
            validation_trajectoire.AnnulerTrajectoire();
        }
    }

    /*
     * Start est appelée une seule fois au début/au lancement.
     * Ici est initialisé la recherche des objets suivants : les trièdres, les boutons et les robots.
     * Ici est aussi géré l'affichage ou non des objets au démarrage de l'application.
     */
    public void Start()
    {
        // On cherche tous les objets nécessaires : trièdres, robots et boutons.
        aruco_controller = GameObject.Find("ARUWP Controller");
        buttons_translation_rotation = GameObject.Find("Boutons translation et rotation");
        triedre_table_transrot = GameObject.Find("Triedre table transrot");
        triedre_table_calib = GameObject.Find("Triedre table");
        triedre_effecteur = GameObject.Find("Triedre effecteur");
        triedre_robot = GameObject.Find("Triedre Robot");
        triedre_robot_virtuel = GameObject.Find("Triedre robot virtuel");
        button_valider_trajectoire = GameObject.Find("Bouton valider trajectoire");
        button_premier_point = GameObject.Find("Bouton premier point");
        button_annuler_trajectoire = GameObject.Find("Bouton annuler trajectoire");

        // On désactive les objets non nécessaires au début de l'application :
        // - les boutons
        if (buttons_translation_rotation != null)
        {
            buttons_translation_rotation.SetActive(false);
        }

        if (button_valider_trajectoire != null)
        {
            button_valider_trajectoire.SetActive(false);
        }

        if (button_premier_point != null)
        {
            button_premier_point.SetActive(false);
        }

        if (button_annuler_trajectoire != null)
        {
            button_annuler_trajectoire.SetActive(false);
        }

        // - les trièdres
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

        // -le trièdre associé au robot virtuel
        if (triedre_robot_virtuel != null)
        {
            triedre_robot_virtuel.SetActive(false);
        }

        // On efface les robots et le trièdre associé au robot virtuel
        robot_reel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
        robot_virtuel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
    }
}