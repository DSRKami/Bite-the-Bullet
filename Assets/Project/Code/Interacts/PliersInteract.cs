using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PliersInteract : MonoBehaviour
{
    public GameObject teethUI;

    public void DisplayTeethUI()
    {
        if (teethUI != null)
        {
            teethUI.SetActive(true);
        }
    }
}
