using UnityEngine;

public class ThiefLogic : MonoBehaviour
{
    public float amplitude = 0.5f; // Высота покачивания
    public float frequency = 2f;   // Скорость покачивания
    public float rotationSpeed = 50f;

    void Update()
    {
        // Покачивание вверх-вниз
        float newY = transform.localPosition.y + Mathf.Sin(Time.time * frequency) * amplitude * Time.deltaTime;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

        // Вращение, чтобы он выглядел живым
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
