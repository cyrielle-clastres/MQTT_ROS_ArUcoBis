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

    // Le script qui contient le choix de l'emplacement et de l'outil
    public Menu menu;

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

    // Les boutons qui gèrent les menus
    protected GameObject button_retour_calibration;
    protected GameObject buttons_menu_emplacement;
    protected GameObject buttons_menu_outil;
    protected GameObject button_retour_choix_emplacement;
    protected GameObject button_retour_choix_outil;
    protected GameObject buttons_trajectoire_envers;

    // Les différents trièdres de notre espace
    private GameObject triedre_table_transrot;
    private GameObject triedre_table_calib;
    private GameObject triedre_effecteur;
    private GameObject triedre_robot;

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
        menu.emplacement = 0;
        aruco_controller.SetActive(true);

        // On efface les boutons et les trièdres
        buttons_translation_rotation.SetActive(false);
        button_valider_trajectoire.SetActive(false);
        button_premier_point.SetActive(false);
        button_annuler_trajectoire.SetActive(false);
        triedre_table_calib.SetActive(false);
        triedre_table_transrot.SetActive(false);
        triedre_robot.SetActive(false);

        // On remet à 0 la matrice de transformation homogène entre le repère de la table et le repère de la table fait avec la calibration
        placement_cube.mat_table_tablecalib = Matrix4x4.identity;
        
        // On efface la trajectoire qui a été tracée si jamais c'est le cas
        validation_trajectoire.AnnulerTrajectoire();

        // On ne permet plus le déplacement du robot virtuel et de son trièdre en bougeant le robot réel
        robot_virtuel.SetTriedre = false;
        robot_virtuel.SetJoints = false;

        // On efface les robots et le trièdre associé au robot virtuel
        robot_virtuel.first[menu.outil].TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
        triedre_effecteur.SetActive(false);

        // On efface les boutons des menus
        button_retour_calibration.SetActive(false);
        buttons_menu_emplacement.SetActive(false);
        buttons_menu_outil.SetActive(false);
        button_retour_choix_emplacement.SetActive(false);
        button_retour_choix_outil.SetActive(false);

        // On efface les boutons qui gèrent la trajectoire
        button_premier_point.SetActive(false);

        menu.outil = 0;
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

            // On affiche le bouton qui permet de retourner à la calibration
            button_retour_calibration.SetActive(true);

            // On affiche les boutons pour choisir l'emplacement du robot virtuel
            buttons_menu_emplacement.SetActive(true);

            // On efface le robot virtuel où qu'il soit
            robot_virtuel.first[menu.outil].TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
            // On remet les joints à 0 pour que tous les robots soient bien initialisés quand appelés.
            robot_virtuel.SetJoints = false;
        }
    }

    public void RetourCalibration()
    {
        // Si les trièdres sont en mouvement
        if ((placement_cube.fixe == 2) || (placement_cube.fixe == 3) || (placement_cube.fixe == 4))
        {
            placement_cube.fixe = 1;
            menu.emplacement = 0;

            // On affiche les trièdres pour le déplacement de ceux-ci.
            buttons_translation_rotation.SetActive(true);
            triedre_table_calib.SetActive(true);
            triedre_table_transrot.SetActive(true);

            // On permet le déplacement du robot virtuel et de son trièdre en bougeant le robot réel
            robot_virtuel.SetTriedre = false;
            robot_virtuel.SetJoints = false;

            // On efface le robot virtuel où qu'il soit
            robot_virtuel.first[menu.outil].TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));

            // On efface les tridères de l'effecteur et de la base du robot
            triedre_effecteur.SetActive(false);
            triedre_robot.SetActive(false);

            // On efface la trajectoire qui a été tracée si jamais c'est le cas
            validation_trajectoire.AnnulerTrajectoire();

            // On efface les boutons qui gèrent la trajectoire
            button_premier_point.SetActive(false);

            // On efface le bouton qui permet de retourner à la calibration
            button_retour_calibration.SetActive(false);

            // On efface le bouton qui permet de retourner au menu du choix du robot
            button_retour_choix_emplacement.SetActive(false);

            // On efface les menus si jamais ils étaient actifs
            buttons_menu_emplacement.SetActive(false);
            buttons_menu_outil.SetActive(false);
        }
    }

    public void ChoixEmplacement()
    {
        if(placement_cube.fixe == 2)
        {
            placement_cube.fixe = 3;

            // On cache les boutons qui gèrent l'affichage du menu
            buttons_menu_emplacement.SetActive(false);
            // On affiche le bouton qui permet de retourner au menu du choix du robot
            button_retour_choix_emplacement.SetActive(true);
            // On affiche les boutons pour choisir l'outil
            buttons_menu_outil.SetActive(true);
        }
    }

    public void ChoixOutil()
    {
        if (placement_cube.fixe == 3)
        {
            robot_virtuel.SetJoints = false;
            robot_virtuel.SetTriedre = false;
            menu.count = 0;
            // On affiche le trièdre de la base du robot
            triedre_robot.SetActive(true);
            placement_cube.fixe = 4;

            // On affiche le trièdre à l'effecteur du robot
            triedre_effecteur.SetActive(true);

            if (menu.emplacement == 1)
            {
                // On affiche le trièdre associé au robot virtuel
                triedre_effecteur.GetComponent<Collider>().enabled = true;

                // On affiche les boutons qui gèrent la trajectoire
                button_premier_point.SetActive(true);
            }

            else if (menu.emplacement == 2)
            {
                triedre_effecteur.GetComponent<Collider>().enabled = false;
            }

            // On cache les boutons qui gèrent l'affichage du menu
            buttons_menu_outil.SetActive(false);
            // On affiche le bouton qui permet de retourner au menu du choix du robot
            button_retour_choix_outil.SetActive(true);
        }
    }

    public void RetourChoixEmplacement()
    {
        if((placement_cube.fixe == 3) || (placement_cube.fixe == 4))
        {
            placement_cube.fixe = 2;
            menu.emplacement = 0;
            // On efface le trièdre de la base du robot
            triedre_robot.SetActive(false);

            // On permet le déplacement du robot virtuel et de son trièdre en bougeant le robot réel
            robot_virtuel.SetTriedre = false;
            robot_virtuel.SetJoints = false;

            // On efface la trajectoire qui a été tracée si jamais c'est le cas
            validation_trajectoire.AnnulerTrajectoire();

            // On efface les boutons qui gèrent la trajectoire
            button_premier_point.SetActive(false);

            // On efface le trièdre à l'effecteur du robot
            triedre_effecteur.SetActive(false);
            triedre_effecteur.GetComponent<Collider>().enabled = false;

            buttons_menu_emplacement.SetActive(true);
            button_retour_choix_emplacement.SetActive(false);
            buttons_menu_outil.SetActive(false);
            button_retour_choix_outil.SetActive(false);

            // On efface le robot virtuel où qu'il soit
            robot_virtuel.first[menu.outil].TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
            menu.outil = 0;
        }
    }

    public void RetourChoixOutil()
    {
        if(placement_cube.fixe == 4)
        {
            placement_cube.fixe = 3;
            // On efface le trièdre de la base du robot
            triedre_robot.SetActive(false);

            // On permet le déplacement du robot virtuel et de son trièdre en bougeant le robot réel
            robot_virtuel.SetTriedre = false;
            robot_virtuel.SetJoints = false;

            // On efface la trajectoire qui a été tracée si jamais c'est le cas
            validation_trajectoire.AnnulerTrajectoire();

            // On efface les boutons qui gèrent la trajectoire
            button_premier_point.SetActive(false);

            // On efface le trièdre à l'effecteur du robot
            triedre_effecteur.SetActive(false);
            triedre_effecteur.GetComponent<Collider>().enabled = false;

            buttons_menu_outil.SetActive(true);
            button_retour_choix_outil.SetActive(false);

            // On efface le robot virtuel où qu'il soit
            robot_virtuel.first[menu.outil].TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
            menu.outil = 0;
        }
    }

    public void TrajectoireEnvers()
    {
        buttons_trajectoire_envers.SetActive(true);
    }

    public void FinTrajectoireEnvers()
    {
        placement_cube.fixe = 4;
        buttons_trajectoire_envers.SetActive(false);
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
        button_valider_trajectoire = GameObject.Find("Bouton valider trajectoire");
        button_premier_point = GameObject.Find("Bouton premier point");
        button_annuler_trajectoire = GameObject.Find("Bouton annuler trajectoire");
        button_retour_calibration = GameObject.Find("Bouton retour calibration");
        buttons_menu_emplacement = GameObject.Find("Boutons menu emplacement");
        buttons_menu_outil = GameObject.Find("Boutons menu outil");
        button_retour_choix_outil = GameObject.Find("Bouton retour choix outil");
        button_retour_choix_emplacement = GameObject.Find("Bouton retour choix emplacement");
        buttons_trajectoire_envers = GameObject.Find("Rejouer trajectoire");

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

        if (button_retour_calibration != null)
        {
            button_retour_calibration.SetActive(false);
        }

        if (buttons_menu_emplacement != null)
        {
            buttons_menu_emplacement.SetActive(false);
        }

        if (buttons_menu_outil != null)
        {
            buttons_menu_outil.SetActive(false);
        }

        if (button_retour_choix_emplacement != null)
        {
            button_retour_choix_emplacement.SetActive(false);
        }

        if (button_retour_choix_outil != null)
        {
            button_retour_choix_outil.SetActive(false);
        }

        if (buttons_trajectoire_envers != null)
        {
            buttons_trajectoire_envers.SetActive(false);
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

        // On efface les robots et le trièdre associé au robot virtuel
        for (int i = 0; i<robot_virtuel.ur3e.Length; i++)
        {
            robot_virtuel.first[i].TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
        }
    }
}