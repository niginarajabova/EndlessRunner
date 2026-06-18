using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float destroyTime = 1f;
    private float moveSpeed = 2f;
    private Camera mainCamera;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        mainCamera = Camera.main;
    }

    public void Setup(int scoreValue)
    {
        if (textMesh != null)
        {
            textMesh.SetText("+{0}", scoreValue);
        }

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // Yuqoriga harakatlanish
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Billboard — doimo kameraga qarab turish
        if (mainCamera != null)
        {
            transform.forward = mainCamera.transform.forward;
        }
    }
}
