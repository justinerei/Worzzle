using UnityEngine;
using System.Collections;

public class UIShaker : MonoBehaviour
{
    public float duration = 0.2f;
    public float magnitude = 10f;

    private RectTransform rectTransform;
    private Vector3 originalPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
    }

    public void Shake()
    {
        StopAllCoroutines(); // In case it's already shaking
        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            rectTransform.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
    }
}
