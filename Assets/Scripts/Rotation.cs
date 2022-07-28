/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Août 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Ce script calcule la matrice de transformation homogène entre le trièdre de la table issu de la calibration et le trièdre de la table dû aux
 * diverses rotations effectuées. Il permet également d'afficher le nouveau trièdre de la table.
 * Il calcule également les nouvelles matrices de transformation homogène entre la table et le monde et entre le monde et la table.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class Rotation : MonoBehaviour
{
    public PlacementCube placement_cube;
    public GameObject triedre_table_transrot;

    /*
     * PlusX est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void ClockwiseX()
    {
        // Matrice de la rotation +x
        Matrix4x4 rot_c_x = Matrix4x4.identity;
        rot_c_x.SetColumn(1, new Vector4(0, Mathf.Cos(1 * Mathf.Deg2Rad), Mathf.Sin(1 * Mathf.Deg2Rad), 0));
        rot_c_x.SetColumn(2, new Vector4(0, -Mathf.Sin(1 * Mathf.Deg2Rad), Mathf.Cos(1 * Mathf.Deg2Rad), 0));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * rot_c_x;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_tablecalib_monde * placement_cube.mat_table_tablecalib;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.rotation = placement_cube.mat_table_monde.rotation;
    }

    /*
     * MinusX est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void CounterClockwiseX()
    {
        // Matrice de la rotation -x
        Matrix4x4 rot_cc_x = Matrix4x4.identity;
        rot_cc_x.SetColumn(1, new Vector4(0, Mathf.Cos(-1 * Mathf.Deg2Rad), Mathf.Sin(-1 * Mathf.Deg2Rad), 0));
        rot_cc_x.SetColumn(2, new Vector4(0, -Mathf.Sin(-1 * Mathf.Deg2Rad), Mathf.Cos(-1 * Mathf.Deg2Rad), 0));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * rot_cc_x;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_tablecalib_monde * placement_cube.mat_table_tablecalib;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.rotation = placement_cube.mat_table_monde.rotation;
    }

    /*
     * PlusY est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void ClockwiseY()
    {
        // Matrice de la rotation +y
        Matrix4x4 rot_c_y = Matrix4x4.identity;
        rot_c_y.SetColumn(0, new Vector4(Mathf.Cos(1 * Mathf.Deg2Rad), 0, - Mathf.Sin(1 * Mathf.Deg2Rad), 0));
        rot_c_y.SetColumn(2, new Vector4(Mathf.Sin(1 * Mathf.Deg2Rad), 0, Mathf.Cos(1 * Mathf.Deg2Rad), 0));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * rot_c_y;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_tablecalib_monde * placement_cube.mat_table_tablecalib;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.rotation = placement_cube.mat_table_monde.rotation;
    }

    /*
     * MinusY est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void CounterClockwiseY()
    {
        // Matrice de la rotation -y
        Matrix4x4 rot_cc_y = Matrix4x4.identity;
        rot_cc_y.SetColumn(0, new Vector4(Mathf.Cos(-1 * Mathf.Deg2Rad), 0, -Mathf.Sin(-1 * Mathf.Deg2Rad), 0));
        rot_cc_y.SetColumn(2, new Vector4(Mathf.Sin(-1 * Mathf.Deg2Rad), 0, Mathf.Cos(-1 * Mathf.Deg2Rad), 0));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * rot_cc_y;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_tablecalib_monde * placement_cube.mat_table_tablecalib;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.rotation = placement_cube.mat_table_monde.rotation;
    }

    /*
     * PlusZ est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void ClockwiseZ()
    {
        // Matrice de la rotation +z
        Matrix4x4 rot_c_z = Matrix4x4.identity;
        rot_c_z.SetColumn(0, new Vector4(Mathf.Cos(1 * Mathf.Deg2Rad), Mathf.Sin(1 * Mathf.Deg2Rad), 0, 0));
        rot_c_z.SetColumn(1, new Vector4(-Mathf.Sin(1 * Mathf.Deg2Rad), Mathf.Cos(1 * Mathf.Deg2Rad), 0, 0));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * rot_c_z;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_tablecalib_monde * placement_cube.mat_table_tablecalib;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.rotation = placement_cube.mat_table_monde.rotation;
    }

    /*
     * MinusZ est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void CounterClockwiseZ()
    {
        // Matrice de la rotation +z
        Matrix4x4 rot_cc_z = Matrix4x4.identity;
        rot_cc_z.SetColumn(0, new Vector4(Mathf.Cos(-1 * Mathf.Deg2Rad), Mathf.Sin(-1 * Mathf.Deg2Rad), 0, 0));
        rot_cc_z.SetColumn(1, new Vector4(-Mathf.Sin(-1 * Mathf.Deg2Rad), Mathf.Cos(-1 * Mathf.Deg2Rad), 0, 0));
        Debug.Log(Mathf.Cos(-1 * Mathf.Deg2Rad));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * rot_cc_z;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_tablecalib_monde * placement_cube.mat_table_tablecalib;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.rotation = placement_cube.mat_table_monde.rotation;
    }
}
