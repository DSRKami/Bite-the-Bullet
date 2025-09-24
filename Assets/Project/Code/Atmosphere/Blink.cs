using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour
{
    public GameObject eyes; // The GameObject representing the eyes
    public float interval = 10f;
    [Range(0f, 1f)]
    public float blinkChance = 0.5f;
    public float blinkDuration = 0.15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if (Random.value < blinkChance)
            {
                eyes.SetActive(false); // Close eyes
                yield return new WaitForSeconds(blinkDuration);
                eyes.SetActive(true); // Open eyes
            }
        }
    }
}
