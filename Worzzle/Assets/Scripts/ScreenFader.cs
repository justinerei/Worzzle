using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image redOverlay;

    [Tooltip("Alpha to fade to (0 = transparent, 1 = solid)")]
    public float targetAlpha = 0.5f;

    [Tooltip("Fade duration in seconds")]
    public float fadeDuration = 0.3f;

    [Tooltip("How long to hold before fading back out")]
    public float holdTime = 0.5f;

    public void FadeToRed()
    {
        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        // Fade in
        yield return StartCoroutine(FadeAlpha(0f, targetAlpha));

        // Hold red
        yield return new WaitForSeconds(holdTime);

        // Fade out
        yield return StartCoroutine(FadeAlpha(targetAlpha, 0f));
    }

    private IEnumerator FadeAlpha(float fromAlpha, float toAlpha)
    {
        float time = 0f;
        Color color = redOverlay.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, time / fadeDuration); // Smooth transition
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            redOverlay.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Snap to final value
        redOverlay.color = new Color(color.r, color.g, color.b, toAlpha);
    }
}
