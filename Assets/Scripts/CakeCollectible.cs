using UnityEngine;

public class CakeCollectible : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 1.5f;
    private float startY;

    void Start() => startY = transform.localPosition.y;

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }

    // ЭТОТ БЛОК ОТВЕЧАЕТ ЗА ИСЧЕЗНОВЕНИЕ
    private void OnTriggerEnter(Collider other) 
    {
        // Проверь, чтобы тег в кавычках в точности (с большой буквы) совпадал с тегом в Unity
        if (other.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}
