using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public Image flashImage;
    [Range(0f, 1f)] public float peakAlpha = 0.35f;
    public float inTime = 0.05f;
    public float holdTime = 0.05f;
    public float outTime = 0.2f;
    public bool isFlashing;

    public void Flash()
    {
        if (!gameObject.activeInHierarchy || flashImage == null) return;
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        isFlashing = true;
        flashImage.gameObject.SetActive(true);

        // Fade In
        float t = 0f;
        Color c = flashImage.color;
        while (t < inTime)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(0f, peakAlpha, t / inTime);
            flashImage.color = c;
            yield return null;
        }
        c.a = peakAlpha;
        flashImage.color = c;

        // Hold
        yield return new WaitForSecondsRealtime(holdTime);

        // Fade Out
        t = 0f;
        while (t < outTime)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(peakAlpha, 0f, t / outTime);
            flashImage.color = c;
            yield return null;
        }
        c.a = 0f;
        flashImage.color = c;
        flashImage.gameObject.SetActive(false);
        isFlashing = false;
    }
}
