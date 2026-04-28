using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float rotateSpeed = 70f;
    private Vector3 visualCenter;

    void Start()
    {
        // Находим реальный центр всех кусочков печенья
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        
        foreach (Renderer r in rends)
        {
            bounds.Encapsulate(r.bounds);
        }
        
        visualCenter = bounds.center; // Это и есть «золотая середина» печенья
    }

    void Update()
    {
        // Вращаем вокруг вычисленного центра, а не вокруг Pivot
        transform.RotateAround(visualCenter, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
