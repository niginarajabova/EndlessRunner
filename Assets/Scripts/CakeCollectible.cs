using UnityEngine;

public class CakeCollectible : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 1.5f;

    private float startY;
    private float timeOffset;

    void Start()
    {
        startY = transform.localPosition.y;
        // Har bir tort alohida fazada suzishi uchun random offset
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        float newY = startY + Mathf.Sin((Time.time + timeOffset) * frequency) * amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }

    // OnTriggerEnter olib tashlandi — PlayerMovement.CollectItem o'zi Destroy qiladi.
    // Bu skript faqat vizual animatsiya uchun javobgar.
}
