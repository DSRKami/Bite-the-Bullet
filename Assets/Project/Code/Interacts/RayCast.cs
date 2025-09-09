using UnityEngine;
using UnityEngine.InputSystem;

public class RayCast : MonoBehaviour
{
    public Camera cam;
    public GameObject pliersHoverUI;
    public GameObject revolverHoverUI;
    public GameObject playerInformationUI;

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
        }

        revolverHoverUI.SetActive(hoveringRevolver);
        pliersHoverUI.SetActive(hoveringPliers);
        playerInformationUI.SetActive(!hoveringRevolver && !hoveringPliers);

        // Click Logic
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Physics.Raycast(ray, out var hitClick, 100f))
            {
                // Revolver Interact
                var revolverInteract = hitClick.collider.GetComponent<RevolverInteract>();
                if (revolverInteract != null && !revolverInteract.isAnimating)
                    revolverInteract.StartCoroutine(revolverInteract.AnimateAndFire());

                // Pliers Interact
                var pliersInteract = hitClick.collider.GetComponent<PliersInteract>();
                if (pliersInteract != null)
                    pliersInteract.DisplayTeethUI();
            }
        }
    }
}