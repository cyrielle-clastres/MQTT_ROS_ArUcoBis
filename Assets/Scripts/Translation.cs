/*
 * Auteure : Cyrielle Clastres
 * Juin, Juillet, Ao�t 2022
 * Contact : cyrielleclatres@gmail.com
 * 
 * Ce script calcule la matrice de transformation homog�ne entre le tri�dre de la table issu de la calibration et le tri�dre de la table d� aux
 * diverses translations effectu�es. Il permet �galement d'afficher le nouveau tri�dre de la table.
 * Il calcule �galement les nouvelles matrices de transformation homog�ne entre la table et le monde et entre le monde et la table.
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
     * PlusX est appel�e lorsque l'utilisateur appuie sur le bouton associ�.
     * Cette fonction calcule la nouvelle matrice de transformation homog�ne entre le tri�dre de la table et le tri�dre issu de la calibration.
     * Elle calcule �galement la nouvelle matrice de transformation homog�ne entre la table et le monde et entre le monde et la table.
     * Elle permet �galement d'afficher le nouveau tri�dre.
     */
    public void PlusX()
    {
        // D�placement effectu� en appuyant sur le bouton de 2 mm selon +x
        Vector3 deplacement = placement_cube.directionu1.normalized * 0.002f;

        // Matrice du d�placement +x
        Matrix4x4 trans_p_x = Matrix4x4.identity;
        trans_p_x.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du d�placement total par rapport au tri�dre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_p_x;

        // Matrice correspondant au d�placement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au d�placement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On d�place le tri�dre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0,3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * MinusX est appel�e lorsque l'utilisateur appuie sur le bouton associ�.
     * Cette fonction calcule la nouvelle matrice de transformation homog�ne entre le tri�dre de la table et le tri�dre issu de la calibration.
     * Elle calcule �galement la nouvelle matrice de transformation homog�ne entre la table et le monde et entre le monde et la table.
     * Elle permet �galement d'afficher le nouveau tri�dre.
     */
    public void MinusX()
    {
        // D�placement effectu� en appuyant sur le bouton de 2 mm selon -x
        Vector3 deplacement = - placement_cube.directionu1.normalized * 0.002f;
        
        // Matrice du d�placement -x
        Matrix4x4 trans_m_x = Matrix4x4.identity;
        trans_m_x.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du d�placement total par rapport au tri�dre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_m_x;

        // Matrice correspondant au d�placement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au d�placement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On d�place le tri�dre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * PlusY est appel�e lorsque l'utilisateur appuie sur le bouton associ�.
     * Cette fonction calcule la nouvelle matrice de transformation homog�ne entre le tri�dre de la table et le tri�dre issu de la calibration.
     * Elle calcule �galement la nouvelle matrice de transformation homog�ne entre la table et le monde et entre le monde et la table.
     * Elle permet �galement d'afficher le nouveau tri�dre.
     */
    public void PlusY()
    {
        // D�placement effectu� en appuyant sur le bouton de 2 mm selon +y
        Vector3 deplacement = placement_cube.normal.normalized * 0.002f;

        // Matrice du d�placement +y
        Matrix4x4 trans_p_y = Matrix4x4.identity;
        trans_p_y.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du d�placement total par rapport au tri�dre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_p_y;

        // Matrice correspondant au d�placement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;
        // Matrice correspondant au d�placement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On d�place le tri�dre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * MinusY est appel�e lorsque l'utilisateur appuie sur le bouton associ�.
     * Cette fonction calcule la nouvelle matrice de transformation homog�ne entre le tri�dre de la table et le tri�dre issu de la calibration.
     * Elle calcule �galement la nouvelle matrice de transformation homog�ne entre la table et le monde et entre le monde et la table.
     * Elle permet �galement d'afficher le nouveau tri�dre.
     */
    public void MinusY()
    {
        // D�placement effectu� en appuyant sur le bouton de 2 mm selon -y
        Vector3 deplacement = - placement_cube.normal.normalized * 0.002f;

        // Matrice du d�placement -y
        Matrix4x4 trans_m_y = Matrix4x4.identity;
        trans_m_y.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du d�placement total par rapport au tri�dre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_m_y;

        // Matrice correspondant au d�placement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au d�placement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On d�place le tri�dre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * PlusZ est appel�e lorsque l'utilisateur appuie sur le bouton associ�.
     * Cette fonction calcule la nouvelle matrice de transformation homog�ne entre le tri�dre de la table et le tri�dre issu de la calibration.
     * Elle calcule �galement la nouvelle matrice de transformation homog�ne entre la table et le monde et entre le monde et la table.
     * Elle permet �galement d'afficher le nouveau tri�dre.
     */
    public void PlusZ()
    {
        // D�placement effectu� en appuyant sur le bouton de 2 mm selon +z
        Vector3 deplacement = placement_cube.directionu2.normalized * 0.002f;

        // Matrice du d�placement +z
        Matrix4x4 trans_p_z = Matrix4x4.identity;
        trans_p_z.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du d�placement total par rapport au tri�dre de la calibration
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_p_z;

        // Matrice correspondant au d�placement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au d�placement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On d�place le tri�dre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }

    /*
     * MinusZ est appel�e lorsque l'utilisateur appuie sur le bouton associ�.
     * Cette fonction calcule la nouvelle matrice de transformation homog�ne entre le tri�dre de la table et le tri�dre issu de la calibration.
     * Elle calcule �galement la nouvelle matrice de transformation homog�ne entre la table et le monde et entre le monde et la table.
     * Elle permet �galement d'afficher le nouveau tri�dre.
     */
    public void MinusZ()
    {
        // D�placement effectu� en appuyant sur le bouton de 2 mm selon -z
        Vector3 deplacement = - placement_cube.directionu2.normalized * 0.002f;

        // Matrice du d�placement -z
        Matrix4x4 trans_m_z = Matrix4x4.identity;
        trans_m_z.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        // Matrice du d�placement total par rapport au tri�dre de la calib
        placement_cube.mat_table_tablecalib = placement_cube.mat_table_tablecalib * trans_m_z;

        // Matrice correspondant au d�placement table monde
        placement_cube.mat_table_monde = placement_cube.mat_table_tablecalib * placement_cube.mat_tablecalib_monde;

        // Matrice correspondant au d�placement monde table
        placement_cube.mat_monde_table = placement_cube.mat_table_monde.inverse;

        // On d�place le tri�dre de la table
        triedre_table_transrot.transform.position = new Vector3(placement_cube.mat_table_monde[0, 3], placement_cube.mat_table_monde[1, 3], placement_cube.mat_table_monde[2, 3]);
    }
}
