using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float destroyTime = 1f; 
    private float moveSpeed = 2f;    

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int scoreValue)
    {
        if (textMesh != null)
        {
            textMesh.text = "+" + scoreValue;
        }
        
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }
}