using UnityEngine;
using System.Collections;

public class PopUpObstacle : MonoBehaviour
{
    public float targetScale = 1f;
    public float growSpeed = 4f;
    public float bounciness = 1.4f;
    public float shakeAmount = 0.1f;

    [Header("Debug")]
    public bool showDebugLogs = false; // Default off — ko'p obstacle bo'lishi mumkin

    void Start()
    {
        transform.localScale = Vector3.zero;

        if (showDebugLogs)
            Debug.Log("[PopUpObstacle] Spawned: " + gameObject.name + " at " + transform.position + " | TargetScale: " + targetScale);

        StartCoroutine(AppearWithJelloEffect());
    }

    IEnumerator AppearWithJelloEffect()
    {
        float t = 0;
        Vector3 finalScale = Vector3.one * targetScale;
        Vector3 overshootScale = finalScale * bounciness;

        // 1. O'sish (overshoot bilan)
        while (t < 1)
        {
            t += Time.deltaTime * growSpeed;
            transform.localScale = Vector3.Lerp(Vector3.zero, overshootScale, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        // 2. Normal o'lchamga qaytish
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * growSpeed;
            transform.localScale = Vector3.Lerp(overshootScale, finalScale, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        // 3. Jele titroq effekti
        for (int i = 0; i < 3; i++)
        {
            float duration = 0.2f;
            float elapsed = 0;
            Vector3 wobbleScale = finalScale + new Vector3(shakeAmount, -shakeAmount, shakeAmount) / (i + 1);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, wobbleScale, elapsed / duration);
                yield return null;
            }

            elapsed = 0;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, finalScale, elapsed / duration);
                yield return null;
            }
        }

        transform.localScale = finalScale;

        if (showDebugLogs)
            Debug.Log("[PopUpObstacle] Animation complete: " + gameObject.name);
    }
}
