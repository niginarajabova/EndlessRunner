using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float rotateSpeed = 70f;

    [Header("Debug")]
    public bool showDebugLogs = false; // Default off — ko'p obyekt bo'lishi mumkin

    void Start()
    {
        if (showDebugLogs)
            Debug.Log("[SimpleRotation] Rotating: " + gameObject.name + " at speed: " + rotateSpeed + "°/s");
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.Self);
    }
}
