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
    public GameObject muzzleFlash;

    public static bool revolverAnimating = false;
    public GameObject enemyEyes;

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

    public void PlayRevolverTurn()
    {
        if (gameManager != null)
        {
            gameManager.PlayTurn(PlayerAction.Revolver);
        }
        if (!revolver.isBlank)
        {
            StartCoroutine(FlashMuzzle());
            WinceinPain();
        }
    }

    public void PlayNextTurn()
    {
        if (gameManager != null)
        {
            gameManager.NextTurn();
        }
    }


    public void BeginAnimation()
    {
        revolverAnimating = true;
    }

    public void EndAnimation()
    {
        revolverAnimating = false;
    }

    IEnumerator FlashMuzzle()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    // Closes the enemies eyes in pain when the player shoots a bullet
    public void WinceinPain()
    {
        StartCoroutine(Wince());
    }
    
    IEnumerator Wince()
    {
        enemyEyes.SetActive(false);
        yield return new WaitForSeconds(3f);
        enemyEyes.SetActive(true);
    }
}   
