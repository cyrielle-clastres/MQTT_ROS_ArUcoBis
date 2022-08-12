using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatricesOutils : MonoBehaviour
{
    // Les matrices de transformation entre les outils et l'extrémité du robot
    public Matrix4x4 mat_base_link_base = Matrix4x4.identity;
    public Matrix4x4 mat_flange_tool0 = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_tool0_flange = Matrix4x4.identity;
    public Matrix4x4 mat_tool0_feutre = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_feutre_tool0 = Matrix4x4.identity;
    public Matrix4x4 mat_tool0_cercle20 = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_cercle20_tool0 = Matrix4x4.identity;
    public Matrix4x4 mat_tool0_cercle40 = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_cercle40_tool0 = Matrix4x4.identity;
    public Matrix4x4 mat_tool0_cercle50 = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_cercle50_tool0 = Matrix4x4.identity;


    // Start is called before the first frame update
    void Start()
    {
        mat_tool0_flange = mat_flange_tool0.inverse;
        mat_feutre_tool0 = mat_tool0_feutre.inverse;
        mat_cercle20_tool0 = mat_tool0_cercle20.inverse;
        mat_cercle40_tool0 = mat_tool0_cercle40.inverse;
        mat_cercle50_tool0 = mat_tool0_cercle50.inverse;
    }
}
