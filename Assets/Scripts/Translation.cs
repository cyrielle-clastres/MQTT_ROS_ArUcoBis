/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Août 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Ce script calcule la matrice de transformation homogène entre le trièdre de la table issu de la calibration et le trièdre de la table dû aux
 * diverses translations effectuées. Il permet également d'afficher le nouveau trièdre de la table.
 * Il calcule également les nouvelles matrices de transformation homogène entre la table et le monde et entre le monde et la table.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class Translation : MonoBehaviour
{
    public PlacementCube placement_cube;
    public GameObject triedre_table_transrot;

    /*
     * PlusX est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void PlusX()
    {
        // Déplacement effectué en appuyant sur le bouton de 2 mm selon +x
        Vector3 deplacement = placement_cube.directionu1.normalized * 0.002f;

        // Matrice du déplacement +x
        Matrix4x4 trans_p_x = Matrix4x4.identity;
        trans_p_x.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_p_x;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0,3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * MinusX est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void MinusX()
    {
        // Déplacement effectué en appuyant sur le bouton de 2 mm selon -x
        Vector3 deplacement = - placement_cube.directionu1.normalized * 0.002f;
        
        // Matrice du déplacement -x
        Matrix4x4 trans_m_x = Matrix4x4.identity;
        trans_m_x.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_m_x;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * PlusY est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void PlusY()
    {
        // Déplacement effectué en appuyant sur le bouton de 2 mm selon +y
        Vector3 deplacement = placement_cube.normal.normalized * 0.002f;

        // Matrice du déplacement +y
        Matrix4x4 trans_p_y = Matrix4x4.identity;
        trans_p_y.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_p_y;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;
        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * MinusY est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void MinusY()
    {
        // Déplacement effectué en appuyant sur le bouton de 2 mm selon -y
        Vector3 deplacement = - placement_cube.normal.normalized * 0.002f;

        // Matrice du déplacement -y
        Matrix4x4 trans_m_y = Matrix4x4.identity;
        trans_m_y.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_m_y;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * PlusZ est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void PlusZ()
    {
        // Déplacement effectué en appuyant sur le bouton de 2 mm selon +z
        Vector3 deplacement = placement_cube.directionu2.normalized * 0.002f;

        // Matrice du déplacement +z
        Matrix4x4 trans_p_z = Matrix4x4.identity;
        trans_p_z.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du déplacement total par rapport au trièdre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_p_z;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * MinusZ est appelée lorsque l'utilisateur appuie sur le bouton associé.
     * Cette fonction calcule la nouvelle matrice de transformation homogène entre le trièdre de la table et le trièdre issu de la calibration.
     * Elle calcule également la nouvelle matrice de transformation homogène entre la table et le monde et entre le monde et la table.
     * Elle permet également d'afficher le nouveau trièdre.
     */
    public void MinusZ()
    {
        // Déplacement effectué en appuyant sur le bouton de 2 mm selon -z
        Vector3 deplacement = - placement_cube.directionu2.normalized * 0.002f;

        // Matrice du déplacement -z
        Matrix4x4 trans_m_z = Matrix4x4.identity;
        trans_m_z.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du déplacement total par rapport au trièdre de la calib
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_m_z;

        // Matrice correspondant au déplacement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au déplacement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On déplace le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }
}
