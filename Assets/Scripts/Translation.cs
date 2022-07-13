using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class Translation : MonoBehaviour
{
    public PlacementCube scriptinverse;
    public GameObject triedre_table_transrot;

    public void PlusX()
    {
        //Calcul de la nouvelle matrice de transformation et de son inverse
        //Matrice du déplacement +x
        Vector3 deplacement = scriptinverse.directionu1.normalized * 0.002f;
        Matrix4x4 trans_p_x = Matrix4x4.identity;
        trans_p_x.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));
        //Matrice du déplacement total par rapport au trièdre de la calib
        scriptinverse.mat_table_tablecalib = scriptinverse.mat_table_tablecalib * trans_p_x;
        //Matrice correspondant au déplacement table monde
        scriptinverse.mat_table_monde = scriptinverse.mat_table_tablecalib * scriptinverse.mat_tablecalib_monde;
        //Matrice correspondant au déplacement monde table
        scriptinverse.mat_monde_table = scriptinverse.mat_table_monde.inverse;

        //Déplacer le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(scriptinverse.mat_table_monde[0,3], scriptinverse.mat_table_monde[1, 3], scriptinverse.mat_table_monde[2, 3]);
    }

    public void MinusX()
    {
        //Calcul de la nouvelle matrice de transformation et de son inverse
        //Matrice du déplacement -x
        Vector3 deplacement = - scriptinverse.directionu1.normalized * 0.002f;
        Matrix4x4 trans_m_x = Matrix4x4.identity;
        trans_m_x.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));
        //Matrice du déplacement total par rapport au trièdre de la calib
        scriptinverse.mat_table_tablecalib = scriptinverse.mat_table_tablecalib * trans_m_x;
        //Matrice correspondant au déplacement table monde
        scriptinverse.mat_table_monde = scriptinverse.mat_table_tablecalib * scriptinverse.mat_tablecalib_monde;
        //Matrice correspondant au déplacement monde table
        scriptinverse.mat_monde_table = scriptinverse.mat_table_monde.inverse;

        //Déplacer le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(scriptinverse.mat_table_monde[0, 3], scriptinverse.mat_table_monde[1, 3], scriptinverse.mat_table_monde[2, 3]);
    }

    public void PlusY()
    {
        //Calcul de la nouvelle matrice de transformation et de son inverse
        //Matrice du déplacement +y
        Vector3 deplacement = scriptinverse.normal.normalized * 0.002f;
        Matrix4x4 trans_p_y = Matrix4x4.identity;
        trans_p_y.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));
        //Matrice du déplacement total par rapport au trièdre de la calib
        scriptinverse.mat_table_tablecalib = scriptinverse.mat_table_tablecalib * trans_p_y;
        //Matrice correspondant au déplacement table monde
        scriptinverse.mat_table_monde = scriptinverse.mat_table_tablecalib * scriptinverse.mat_tablecalib_monde;
        //Matrice correspondant au déplacement monde table
        scriptinverse.mat_monde_table = scriptinverse.mat_table_monde.inverse;

        //Déplacer le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(scriptinverse.mat_table_monde[0, 3], scriptinverse.mat_table_monde[1, 3], scriptinverse.mat_table_monde[2, 3]);
    }

    public void MinusY()
    {
        //Calcul de la nouvelle matrice de transformation et de son inverse
        //Matrice du déplacement -y
        Vector3 deplacement = - scriptinverse.normal.normalized * 0.002f;
        Matrix4x4 trans_m_y = Matrix4x4.identity;
        trans_m_y.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));
        //Matrice du déplacement total par rapport au trièdre de la calib
        scriptinverse.mat_table_tablecalib = scriptinverse.mat_table_tablecalib * trans_m_y;
        //Matrice correspondant au déplacement table monde
        scriptinverse.mat_table_monde = scriptinverse.mat_table_tablecalib * scriptinverse.mat_tablecalib_monde;
        //Matrice correspondant au déplacement monde table
        scriptinverse.mat_monde_table = scriptinverse.mat_table_monde.inverse;

        //Déplacer le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(scriptinverse.mat_table_monde[0, 3], scriptinverse.mat_table_monde[1, 3], scriptinverse.mat_table_monde[2, 3]);
    }

    public void PlusZ()
    {
        //Calcul de la nouvelle matrice de transformation et de son inverse
        //Matrice du déplacement +z
        Vector3 deplacement = scriptinverse.directionu2.normalized * 0.002f;
        Matrix4x4 trans_p_z = Matrix4x4.identity;
        trans_p_z.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));
        //Matrice du déplacement total par rapport au trièdre de la calib
        scriptinverse.mat_table_tablecalib = scriptinverse.mat_table_tablecalib * trans_p_z;
        //Matrice correspondant au déplacement table monde
        scriptinverse.mat_table_monde = scriptinverse.mat_table_tablecalib * scriptinverse.mat_tablecalib_monde;
        //Matrice correspondant au déplacement monde table
        scriptinverse.mat_monde_table = scriptinverse.mat_table_monde.inverse;

        //Déplacer le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(scriptinverse.mat_table_monde[0, 3], scriptinverse.mat_table_monde[1, 3], scriptinverse.mat_table_monde[2, 3]);
    }

    public void MinusZ()
    {
        //Calcul de la nouvelle matrice de transformation et de son inverse
        //Matrice du déplacement -y
        Vector3 deplacement = - scriptinverse.directionu2.normalized * 0.002f;
        Matrix4x4 trans_m_z = Matrix4x4.identity;
        trans_m_z.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));
        //Matrice du déplacement total par rapport au trièdre de la calib
        scriptinverse.mat_table_tablecalib = scriptinverse.mat_table_tablecalib * trans_m_z;
        //Matrice correspondant au déplacement table monde
        scriptinverse.mat_table_monde = scriptinverse.mat_table_tablecalib * scriptinverse.mat_tablecalib_monde;
        //Matrice correspondant au déplacement monde table
        scriptinverse.mat_monde_table = scriptinverse.mat_table_monde.inverse;

        //Déplacer le trièdre de la table
        triedre_table_transrot.transform.position = new Vector3(scriptinverse.mat_table_monde[0, 3], scriptinverse.mat_table_monde[1, 3], scriptinverse.mat_table_monde[2, 3]);
    }
}
