/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Août 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Ce script calcule le milieu des 4 ArUcos, place le trièdre correspondant et l'oriente. Il calcule les 3 directions correspondant aux vecteurs de l'espace.
 * Il calcule les matrices de transformations homogènes entre les ArUcos et la table et entre la table et le monde.
 * Il permet de placer les repères de la table et du robot. Il place également les boutons pour déplacer le trièdre et pour défixer le trièdre.
 * Enfin, il permet de placer les 2 robots virtuels à la place qui est convenue.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class PlacementCube : MonoBehaviour
{
    // Les 3 directions qui vont permettre de tracer le repère de la table
    public Vector3 directionu1;
    public Vector3 directionu2;
    public Vector3 normal;

    // Permet de fixer le repère de la table : 0 les repères sont libres, 1 le repère de la table est fixe, 2 le repère est fixé en translation
    //[HideInInspector]
    public int fixe = 0;

    // Le trièdre des ArUco et de la table
    protected GameObject triedre_feuille;
    protected GameObject triedre_table;
    protected GameObject triedre_robot;

    // Les trièdres des ArUco
    protected GameObject triedre_haut_gauche;
    protected GameObject triedre_haut_droite;
    protected GameObject triedre_bas_gauche;
    protected GameObject triedre_bas_droite;

    // Le bouton qui permet de "défixer" le trièdre de la table
    protected GameObject button_fixe;
    // Les boutons qui permettent de bouger le trièdre de la table
    protected GameObject buttons_translation_rotation;
    // Le bouton qui permet de valider la trajectoire du robot
    protected GameObject button_valider_trajectoire;
    // Le bouton qui permet de valider le premier point de la trajectoire
    protected GameObject button_premier_point;
    // Le bouton qui permet d'annuler le placement du premier point et la trajectoire tracée
    protected GameObject button_annuler_trajectoire;
    // Les boutons qui permettent de choisir l'emplacement du robot virtuel
    protected GameObject buttons_menu_emplacement;
    // Les boutons qui permettent de choisir l'outil du robot virtuel
    protected GameObject buttons_menu_outil;

    // Les boutons qui servent à rejouer ou non la trajectoire
    protected GameObject buttons_trajectoire_envers;

    // Le script qui permet de faire bouger le robot virtuel selon le déplacement de son trièdre/effecteur (nécessaire pour ancrer le robot virtiel à une place précise)
    public RobotVirtuel robot_virtuel;

    // Le script qui nous indique l'emplacement du robot et l'outil sélectionné
    public Menu menu;

    // La matrice de transformation homogène de la table vers le monde et du monde vers la table
    [HideInInspector]
    public Matrix4x4 mat_tablecalib_monde = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_table_monde = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_monde_table = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_table_tablecalib = Matrix4x4.identity;
    protected Matrix4x4 mat_robot_table = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_robot_monde = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_monde_robot = Matrix4x4.identity;

    /*
     * Start est appelée une seule fois au début/au lancement.
     * Ici, est initialisé la recherche des objets suivants : les trièdres des ArUcos, de la table et de la base du robot.
     * Sont également cherchés les 2 robots virtuels et le bouton pour défixer le trièdre de la table.
     * Ici est également créée la matrice de transformation homogène entre le robot et la table dans le repère indirect.
     */
    void Start()
    {
        // Recherche des objets
        triedre_feuille = GameObject.Find("Triedre feuille");
        triedre_table = GameObject.Find("Triedre table");
        button_fixe = GameObject.Find("Boutons defixer table");
        triedre_robot = GameObject.Find("Triedre Robot");
        button_valider_trajectoire = GameObject.Find("Bouton valider trajectoire");
        button_premier_point = GameObject.Find("Bouton premier point");
        button_annuler_trajectoire = GameObject.Find("Bouton annuler trajectoire");
        buttons_menu_emplacement = GameObject.Find("Boutons menu emplacement");
        buttons_menu_outil = GameObject.Find("Boutons menu outil");
        buttons_trajectoire_envers = GameObject.Find("Rejouer trajectoire");

        // Création de la matrice de transformation homogène entre le robot et la table
        mat_robot_table.SetColumn(0, new Vector4(0, 0, 1, 0));
        mat_robot_table.SetColumn(1, new Vector4(0, 1, 0, 0));
        mat_robot_table.SetColumn(2, new Vector4(-1, 0, 0, 0));
        mat_robot_table.SetColumn(3, new Vector4(0.259f, 0, 0.385f, 1));
    }

    /* Update est appelée à chaque frame.
     * Ici sont initialisés les trièdres posés sur les ArUcos et les boutons servant à la translation.
     * Est calculé aussi la position centrale des ArUcos ainsi que la rotation de ceux-ci.
     */
    void Update()
    {
        // Initialisation des objets
        triedre_haut_gauche = GameObject.Find("Triedre haut gauche");
        triedre_haut_droite = GameObject.Find("Triedre haut droite");
        triedre_bas_gauche = GameObject.Find("Triedre bas gauche");
        triedre_bas_droite = GameObject.Find("Triedre bas droite");
        buttons_translation_rotation = GameObject.Find("Boutons translation et rotation");

        // Calcul de la position centrale et de la rotation des ArUcos.
        if ((triedre_haut_gauche != null) && (triedre_haut_droite != null) && (triedre_bas_gauche != null) && (triedre_bas_droite != null))
        {
            Vector3 pos = new Vector3();
            Quaternion rot = new Quaternion();
            pos.x = (triedre_haut_gauche.transform.position.x + triedre_haut_droite.transform.position.x + triedre_bas_gauche.transform.position.x + triedre_bas_droite.transform.position.x) / 4;
            pos.y = (triedre_haut_gauche.transform.position.y + triedre_haut_droite.transform.position.y + triedre_bas_gauche.transform.position.y + triedre_bas_droite.transform.position.y) / 4;
            pos.z = (triedre_haut_gauche.transform.position.z + triedre_bas_gauche.transform.position.z + triedre_haut_droite.transform.position.z + triedre_bas_droite.transform.position.z) / 4;

            rot.x = (triedre_haut_gauche.transform.rotation.x + triedre_haut_droite.transform.rotation.x + triedre_bas_gauche.transform.rotation.x + triedre_bas_droite.transform.rotation.x) / 4;
            rot.y = (triedre_haut_gauche.transform.rotation.y + triedre_haut_droite.transform.rotation.y + triedre_bas_gauche.transform.rotation.y + triedre_bas_droite.transform.rotation.y) / 4;
            rot.z = (triedre_haut_gauche.transform.rotation.z + triedre_haut_droite.transform.rotation.z + triedre_bas_gauche.transform.rotation.z + triedre_bas_droite.transform.rotation.z) / 4;
            rot.w = (triedre_haut_gauche.transform.rotation.w + triedre_haut_droite.transform.rotation.w + triedre_bas_gauche.transform.rotation.w + triedre_bas_droite.transform.rotation.w) / 4;
            
            TracerTriedres(pos, rot);
        }
    }

    /*
     * Tracer *Triedres est appelée à chaque frame.
     * Fonction qui permet de tracer le repère de la table en direct, le repère des ArUcos en indirect et qui calcule la transformation du repère
     * monde au repère de la table en indirect.
     * Les 3 directions sont calculées ici. Est également calculé ici, la position du bouton permettant de débloquer le trièdre de la table ainsi que la position
     * des boutons permettant de déplacer le trièdre de la table.
     * Ici sont également placés les 2 robots virtuels.
     */
    private void TracerTriedres(Vector3 pos, Quaternion rot)
    {
        // Les trièdres sur les ArUcos sont libres.
        if (fixe == 0)
        {
            // Matrices de transformation
            Matrix4x4 mat_tablecalib_aruco = Matrix4x4.identity;
            Matrix4x4 mat_aruco_monde = Matrix4x4.identity;
            
            // Calcul des 3 directions correspondant aux directions des repères indirect
            // Directionu1 = x et Directionu2 = y Normal = z car Repère Unity en indirect
            directionu1 = triedre_haut_droite.transform.position - triedre_haut_gauche.transform.position;
            directionu2 = triedre_haut_droite.transform.position - triedre_bas_droite.transform.position;
            normal = Vector3.Cross(directionu1, directionu2);

            // Centre est ici le point de pose du trièdre correspondant à la table
            // Les distances sont exprimées en mètres. Les différentes multiplications servent à placer le repère au bon endroit.
            Vector3 pos_table = pos - directionu1.normalized * 1.0f - directionu2.normalized * 0.525f - normal.normalized * 0.63f;

            // On calcule la matrice de transformation homogène pour passer du repère de la table au repère des ArUcos exprimée dans le repère des ArUcos
            // Il s'agit d'une matrice de translation pure
            mat_tablecalib_aruco.SetColumn(0, new Vector4(1, 0, 0, 0));
            mat_tablecalib_aruco.SetColumn(1, new Vector4(0, 1, 0, 0));
            mat_tablecalib_aruco.SetColumn(2, new Vector4(0, 0, 1, 0));
            mat_tablecalib_aruco.SetColumn(3, new Vector4(pos_table.x - pos.x, pos_table.y - pos.y, pos_table.z - pos.z, 1));

            // On calcule la matrice de transformation homogène pour passer du repère des ArUcos au repère du monde
            // Il s'agit d'une matrice de rotation (correspondant aux directions calculées plus haut) et de translation (correspondant au milieu des 4 ArUcos)
            mat_aruco_monde.SetColumn(0, new Vector4((directionu1.normalized).x, (directionu1.normalized).y, (directionu1.normalized).z, 0));
            mat_aruco_monde.SetColumn(1, new Vector4((directionu2.normalized).x, (directionu2.normalized).y, (directionu2.normalized).z, 0));
            mat_aruco_monde.SetColumn(2, new Vector4((normal.normalized).x, (normal.normalized).y, (normal.normalized).z, 0));
            mat_aruco_monde.SetColumn(3, new Vector4(pos.x, pos.y, pos.z, 1));

            // On calcule la matrice de transformation homogène pour passer du repère de la table au repère du monde
            mat_tablecalib_monde = mat_tablecalib_aruco * mat_aruco_monde;

            // On calcule la matrice de transformation homogène pour passer du repère du monde au repère de la table
            mat_monde_table = mat_tablecalib_monde.inverse;

            // On place le trièdre de la table
            triedre_table.transform.position = pos_table;
            triedre_table.transform.rotation = mat_aruco_monde.rotation;

            // On place le trièdre des ArUcos
            // On enlève 3 centimètres en z pour placer le repère sur la feuille
            triedre_feuille.transform.rotation = mat_aruco_monde.rotation;
            triedre_feuille.transform.position = pos - normal.normalized * 0.03f;

            // On place le bouton pour défixer le repère dans l'espace
            // On calcule la distance entre la position précédente et sa nouvelle position du bouton
            float sqrLen = (pos + directionu1.normalized * 0.40f - directionu2.normalized * 0.55f - button_fixe.transform.position).sqrMagnitude;
            // Si l'écart de position est important, on replace le bouton. Cela permet d'avoir un bouton non trop éloigné des ArUcos.
            if (sqrLen > 0.5)
            {
                button_fixe.transform.position = pos + directionu1.normalized * 0.30f - directionu2.normalized * 0.1f - normal.normalized * 0.55f;
                // On ajoute 180 degrés autour de x pour la rotation, de sorte que le bouton soit bien orienté pour l'utilisateur (c'est-à-dire à la verticale).
                Vector3 rot_button = rot.eulerAngles;
                rot_button = new Vector3(rot_button.x + 180, rot_button.y, rot_button.z);
                button_fixe.transform.rotation = Quaternion.Euler(rot_button);
            }
        }

        // On place les boutons de translation de sorte à ce qu'ils soient proches du trièdre de la table et bien orientés pour l'utilisateur
        else if ((fixe == 1) && (buttons_translation_rotation != null) && (menu.emplacement == 0))
        {
            buttons_translation_rotation.transform.position = pos - directionu1.normalized * 1.0f - directionu2.normalized * 0.3f - normal.normalized * 0.63f;
            buttons_translation_rotation.transform.rotation = mat_tablecalib_monde.rotation;

            // Matrice correspondant au déplacement table monde
            mat_table_monde = mat_table_tablecalib * mat_tablecalib_monde;

            // Matrice correspondant au déplacement monde table
            mat_monde_table = mat_table_monde.inverse;

            // On calcule la matrice de transformation homogène globale entre le monde et le robot et entre le robot et le monde.
            mat_robot_monde = mat_table_monde * mat_robot_table;
            mat_monde_robot = mat_robot_monde.inverse;

            // On place les robots virtuels aux positions voulues. On ajoute 90 degrés selon y pour qu'ils sont orientés de la même façon que le robot réel.
            Vector3 rotation = mat_robot_monde.rotation.eulerAngles;
            rotation = new Vector3(rotation.x, rotation.y + 90, rotation.z);
            robot_virtuel.first[menu.outil].TeleportRoot(new Vector3(mat_robot_monde[0, 3] - directionu2.normalized.x * 0.015f, mat_robot_monde[1, 3] - directionu2.normalized.y * 0.015f, mat_robot_monde[2, 3] - directionu2.normalized.z * 0.015f), Quaternion.Euler(rotation));
            }

        else if (fixe == 2)
        {
            buttons_menu_emplacement.transform.position = pos - directionu1.normalized * 0.55f - directionu2.normalized * 0.3f - normal.normalized * 0.63f;
            buttons_menu_emplacement.transform.rotation = mat_tablecalib_monde.rotation;
        }

        else if (fixe == 3)
        {
            buttons_menu_outil.transform.position = pos - directionu1.normalized * 0.55f - directionu2.normalized * 0.3f - normal.normalized * 0.63f;
            buttons_menu_outil.transform.rotation = mat_tablecalib_monde.rotation;
        }

        // Le trièdre de la table est fixé en translation. Le positionnement est terminé, le choix de l'emplacement du robot est fait.
        else if (fixe == 4)
        {
            // On calcule la matrice de transformation homogène globale entre le monde et le robot et entre le robot et le monde.
            mat_robot_monde = mat_table_monde * mat_robot_table;
            mat_monde_robot = mat_robot_monde.inverse;

            // On place le trièdre correspondant à la base du robot
            triedre_robot.transform.rotation = mat_robot_monde.rotation;
            triedre_robot.transform.position = new Vector3(mat_robot_monde[0,3] - directionu2.normalized.x * 0.015f, mat_robot_monde[1,3] - directionu2.normalized.y * 0.015f, mat_robot_monde[2,3] - directionu2.normalized.z * 0.015f);

            // On place les robots virtuels aux positions voulues. On ajoute 90 degrés selon y pour qu'ils sont orientés de la même façon que le robot réel.
            Vector3 rotation = mat_robot_monde.rotation.eulerAngles;
            rotation = new Vector3(rotation.x, rotation.y + 90, rotation.z);
            if(menu.emplacement == 1)
            {
                robot_virtuel.first[menu.outil].TeleportRoot(new Vector3(mat_robot_monde[0, 3] - directionu2.normalized.x * 0.015f, mat_robot_monde[1, 3] - directionu2.normalized.y * 0.015f, mat_robot_monde[2, 3] - directionu2.normalized.z * 0.015f), Quaternion.Euler(rotation));

                if ((button_valider_trajectoire != null) && (button_premier_point != null))
                {
                    button_premier_point.transform.position = pos - directionu1.normalized * 1.0f - directionu2.normalized * 0.1f - normal.normalized * 0.63f;
                    button_premier_point.transform.rotation = mat_table_monde.rotation;

                    button_valider_trajectoire.transform.position = pos - directionu1.normalized * 0.9f - directionu2.normalized * 0.1f - normal.normalized * 0.63f;
                    button_valider_trajectoire.transform.rotation = mat_table_monde.rotation;

                    button_annuler_trajectoire.transform.position = pos - directionu1.normalized * 0.8f - directionu2.normalized * 0.1f - normal.normalized * 0.63f;
                    button_annuler_trajectoire.transform.rotation = mat_table_monde.rotation;
                }
            }
            else if (menu.emplacement == 2)
            {
                robot_virtuel.first[menu.outil].TeleportRoot(new Vector3(mat_robot_monde[0, 3] + directionu1.normalized.x * 0.5f - directionu2.normalized.x * 0.015f, mat_robot_monde[1, 3] + directionu1.normalized.y * 0.5f - directionu2.normalized.y * 0.015f, mat_robot_monde[2, 3] + directionu1.normalized.z * 0.5f - directionu2.normalized.z * 0.015f), Quaternion.Euler(rotation));
            }
        }

        // On demande si on refait la trajectoire à l'envers ou pas
        else if (fixe == 5)
        {
            buttons_trajectoire_envers.transform.position = pos - directionu1.normalized * 0.55f - directionu2.normalized * 0.3f - normal.normalized * 0.63f;
            buttons_trajectoire_envers.transform.rotation = mat_tablecalib_monde.rotation;
        }
    }
}
