using UnityEngine;
using System.Collections;

public class PopUpObstacle : MonoBehaviour
{
    public float targetScale = 1f;    
    public float growSpeed = 4f;      // Скорость появления
    public float bounciness = 1.4f;   // Насколько сильно раздувается в начале
    public float shakeAmount = 0.1f;  // Сила "дрожания" желе
    
    void Start()
    {
        // Сразу при появлении запускаем эффект, не дожидаясь игрока
        transform.localScale = Vector3.zero;
        StartCoroutine(AppearWithJelloEffect());
    }

    IEnumerator AppearWithJelloEffect()
    {
        float t = 0;
        Vector3 finalScale = Vector3.one * targetScale;
        Vector3 overshootScale = finalScale * bounciness;

        // 1. ПЛАВНЫЙ РОСТ (Буп!)
        while (t < 1)
        {
            t += Time.deltaTime * growSpeed;
            transform.localScale = Vector3.Lerp(Vector3.zero, overshootScale, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        // 2. ВОЗВРАТ К НОРМЕ
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * growSpeed;
            transform.localScale = Vector3.Lerp(overshootScale, finalScale, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        // 3. ЭФФЕКТ ЖЕЛЕ (Дрожание после постановки)
        // Оно будет покачиваться еще 3 раза, затухая
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
            
            // В обратную сторону
            elapsed = 0;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, finalScale, elapsed / duration);
                yield return null;
            }
        }
        
        transform.localScale = finalScale; // Фиксируем финальный размер
    }
}
