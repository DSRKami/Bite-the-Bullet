using UnityEngine;
using System.Collections;

public class CameraTurnController : MonoBehaviour
{
    public Transform playerAPos;
    public Transform playerBPos;
    public GameManager gameManager;
    public float transitionDuration = 1.2f;

    public bool lastIsPlayerATurn;
    public bool isBlending;

    void Start()
    {
        
    }

    void Update()
    {
        if (gameManager.decidingPlayer == true)
        {
            lastIsPlayerATurn = gameManager.isPlayerATurn;
            // Snap to the correct side at start
            if (!gameManager.isPlayerATurn)
            {
                Transform t = playerBPos;
                transform.SetPositionAndRotation(t.position, t.rotation);
                GetComponent<CameraWobble>()?.ResetBaseTransform();
            }

            gameManager.decidingPlayer = false;
        }

        // Only react when the turn flips
        if (gameManager.isPlayerATurn != lastIsPlayerATurn && !isBlending)
        {
            Debug.Log("Triggering");
            lastIsPlayerATurn = gameManager.isPlayerATurn;
            Transform target = lastIsPlayerATurn ? playerAPos : playerBPos;
            StopAllCoroutines();
            StartCoroutine(BlendToPosition(target));
        }
    }

    IEnumerator BlendToPosition(Transform target)
    {
        isBlending = true;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / transitionDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }

        // Snap to target
        transform.SetPositionAndRotation(endPos, endRot);
        isBlending = false;
        GetComponent<CameraWobble>()?.ResetBaseTransform();
    }


}
