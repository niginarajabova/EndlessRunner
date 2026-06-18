using UnityEngine;

public class CakeCollectible : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 1.5f;

    [Header("Debug")]
    public bool showDebugLogs = false; // Default off — ko'p obyekt bo'lishi mumkin

    private float startY;
    private float timeOffset;

    void Start()
    {
        startY = transform.localPosition.y;
        timeOffset = Random.Range(0f, Mathf.PI * 2f);

        if (showDebugLogs)
            Debug.Log("[CakeCollectible] Spawned: " + gameObject.name + " at " + transform.position + " | StartY(local): " + startY);
    }

    void Update()
    {
        float newY = startY + Mathf.Sin((Time.time + timeOffset) * frequency) * amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}
