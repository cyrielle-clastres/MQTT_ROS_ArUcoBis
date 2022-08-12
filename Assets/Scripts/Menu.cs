using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [HideInInspector]
    public int emplacement = 0;
    [HideInInspector]
    public string[] nom_outil = new string[5] { "vide", "feutre", "anneau_d20mm", "anneau_d40mm", "anneau_d50mm" };
    public string[] nom_robot = new string[5] { "ur3e_vide", "ur3e_feutre", "ur3e_anneau_d20mm", "ur3e_anneau_d40mm", "ur3e_anneau_d50mm" };
    public int outil = 0;

    public int count = 0;

    public void SurRobot()
    {
        emplacement = 1;
    }

    public void ACoteRobot()
    {
        emplacement = 2;
    }

    public void Vide()
    {
        outil = 0;
    }

    public void Feutre()
    {
        outil = 1;
    }

    public void Anneau_d20mm()
    {
        outil = 2;
    }

    public void Anneau_d40mm()
    {
        outil = 3;
    }

    public void Anneau_d50mm()
    {
        outil = 4;
    }
}
