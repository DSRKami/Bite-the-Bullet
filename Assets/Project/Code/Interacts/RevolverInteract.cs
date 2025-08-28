using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class RevolverInteract : MonoBehaviour
{
    [Header("Poses")]
    public Transform idlePose;
    public Transform aimPose;

    [Header("Timing (in seconds)")]
    public float liftTime = 0.25f;
    public float fireHoldTime = 0.15f;
    public float dropTime = 0.25f;

    [Header("Gameplay")]
    public GameManager gameManager;
    public Revolver revolver;

    bool isAnimating;

    void Start()
    {
        if (idlePose == null)
        {
            // Create a new GameObject to store the idle pose
            GameObject idleObj = new GameObject("IdlePose");
            idleObj.transform.position = transform.position;
            idleObj.transform.rotation = transform.rotation;
            idlePose = idleObj.transform;
        }

    }

    void Reset()
    {
        if (!revolver) revolver = GetComponent<Revolver>();
    }

    public void OnMouseDown()
    {
        if (isAnimating) return;
        Debug.Log("Revolver Interact Clicked");
        StartCoroutine(AnimateAndFire());
    }

    public IEnumerator AnimateAndFire()
    {
        Debug.Log("Revolver Interact AnimateAndFire");

        isAnimating = true;

        // Lift & Aim
        yield return LerpTransform(transform, idlePose, aimPose, liftTime);

        // Hold at Aim
        yield return new WaitForSeconds(fireHoldTime);

        // Fire Gameplay Logic
        ToothType firedTooth = revolver.Fire();
        if (gameManager != null)
        {
            gameManager.PlayTurn(PlayerAction.Revolver);
        }

        // Drop Back down
        yield return LerpTransform(transform, aimPose, idlePose, dropTime);

        isAnimating = false;
    }

    IEnumerator LerpTransform(Transform obj, Transform start, Transform end, float duration)
    {
        float elapsed = 0f;
        Vector3 initialPosition = start.position;
        Quaternion initialRotation = start.rotation;
        Vector3 targetPosition = end.position;
        Quaternion targetRotation = end.rotation;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            obj.position = Vector3.Lerp(initialPosition, targetPosition, t);
            obj.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.position = targetPosition;
        obj.rotation = targetRotation;
    }
}   
