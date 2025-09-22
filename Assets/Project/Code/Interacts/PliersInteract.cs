using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PliersInteract : MonoBehaviour
{   
    public int pliersIndex; // 0 for Player A, 1 for Player B

    public GameObject teethUI;
    public GameObject toothb;
    public static bool pliersAnimating = false;

    public void DisplayTeethUI()
    {
        if (teethUI != null)
        {
            teethUI.SetActive(true);
        }
    }

    public void ShowTooth()
    {
        if (toothb != null)
        {
            toothb.SetActive(true);
        }
    }

    public void HideTooth()
    {
        if (toothb != null)
        {
            toothb.SetActive(false);
        }
    }

    public void BeginAnimation()
    {
        pliersAnimating = true;
    }

    public void EndAnimation()
    {
        pliersAnimating = false;
    }
}
