using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class RayCast : MonoBehaviour
{
    public Camera cam;
    public GameObject objectNameUI;
    public GameObject objectStatus;
    public GameObject objectDescriptionUI;

    [Header("Plier A Information")]
    public Animator pliersAnimator;
    public GameObject toothUI;

    [Header("Revolver A Information")]
    public Animator revolverAnimator;
    public Animator revolverA;
    public Animator revolverB;

    void Update()
    {
        // Hover Logic
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(screenPos);
        bool hoveringRevolver = false;
        bool hoveringPliers = false;

        if (Physics.Raycast(ray, out var hit, 100f))
        {
            if (hit.collider.GetComponent<RevolverInteract>() != null) hoveringRevolver = true;
            if (hit.collider.GetComponent<PliersInteract>() != null) hoveringPliers = true;

            objectNameUI.SetActive(hoveringRevolver || hoveringPliers || TeethUI.toothHovering);
            objectDescriptionUI.SetActive(hoveringRevolver || hoveringPliers || TeethUI.toothHovering);
            objectStatus.SetActive(TeethUI.toothHovering);

            if (hoveringRevolver)
            {
                objectNameUI.GetComponent<TextMeshProUGUI>().text = "REVOLVER";
                objectDescriptionUI.GetComponent<TextMeshProUGUI>().text = "FIRES A CHAMBER BASED ON CURRENT ODDS.";
            }
            else if (hoveringPliers)
            {
                objectNameUI.GetComponent<TextMeshProUGUI>().text = "PLIERS";
                objectDescriptionUI.GetComponent<TextMeshProUGUI>().text = "SELECTS A TOOTH TO LOAD INTO THE REVOLVER.";
            }

            // Click Logic
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out var hitClick, 100f))
                {
                    // Revolver Interact
                    var revolverInteract = hitClick.collider.GetComponent<RevolverInteract>();
                    if (revolverInteract != null)
                    {
                        revolverAnimator = hitClick.collider.GetComponent<Animator>();
                        RevolverInteract.revolverAnimating = true;
                        revolverAnimator.SetTrigger("Shoot");
                        RevolverInteract.revolverAnimating = false;
                    }

                    // Pliers Interact
                    var pliersInteract = hitClick.collider.GetComponent<PliersInteract>();
                    if (pliersInteract != null)
                    {
                        if (pliersInteract.pliersIndex == 0) revolverAnimator = revolverA;
                        else revolverAnimator = revolverB;

                        pliersAnimator = hitClick.collider.GetComponent<Animator>();
                        PliersInteract.pliersAnimating = true;
                        pliersAnimator.SetTrigger("Pick Up Pliers");
                        PliersInteract.pliersAnimating = false;
                    }
                }
            }
        }
    }

    public void DropPliers()
    {
        PliersInteract.pliersAnimating = true;
        pliersAnimator.SetTrigger("Drop Pliers");
        PliersInteract.pliersAnimating = false;
        toothUI.SetActive(false);
    }
}