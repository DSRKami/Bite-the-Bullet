using UnityEngine;
using UnityEngine.InputSystem;

public class RayCast : MonoBehaviour
{
    public Camera cam;

    void Update()
    {
        // Left mouse (new Input System)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Ray ray = cam.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out var hit, 100f))
            {
                // Revolver Interact
                var revolverInteract = hit.collider.GetComponent<RevolverInteract>();
                if (revolverInteract != null && !revolverInteract.isAnimating)
                    revolverInteract.StartCoroutine(revolverInteract.AnimateAndFire());

                // Pliers Interact
                var pliersInteract = hit.collider.GetComponent<PliersInteract>();
                if (pliersInteract != null)
                    pliersInteract.DisplayTeethUI();
            }
        }
    }
}