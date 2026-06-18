using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float destroyTime = 1f;
    private float moveSpeed = 2f;
    private Camera mainCamera;

    [Header("Debug")]
    public bool showDebugLogs = true;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        mainCamera = Camera.main;

        if (showDebugLogs)
        {
            if (textMesh == null)
                Debug.LogWarning("[FloatingText] TextMeshPro component NOT found on " + gameObject.name);
            if (mainCamera == null)
                Debug.LogWarning("[FloatingText] Camera.main is NULL — billboard won't work.");
        }
    }

    public void Setup(int scoreValue)
    {
        if (textMesh != null)
        {
            textMesh.SetText("+{0}", scoreValue);
        }

        if (showDebugLogs)
            Debug.Log("[FloatingText] Showing: +" + scoreValue + " at " + transform.position);

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if (mainCamera != null)
        {
            transform.forward = mainCamera.transform.forward;
        }
    }
}
