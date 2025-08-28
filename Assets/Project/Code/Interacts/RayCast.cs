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
                var interact = hit.collider.GetComponent<RevolverInteract>();
                if (interact != null)
                    interact.StartCoroutine(interact.AnimateAndFire());
                    Debug.Log("RayCast Clicked at: " + screenPos);
            }
        }
    }
}