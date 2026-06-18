using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float rotateSpeed = 70f;

    private Vector3 visualCenterOffset;

    void Start()
    {
        // Vizual markazni LOCAL offset sifatida hisoblash
        // Shunda parent harakatlansa ham to'g'ri ishlaydi
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        if (rends.Length == 0) return;

        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer r in rends)
        {
            bounds.Encapsulate(r.bounds);
        }

        // World center ni local offset ga aylantirish
        visualCenterOffset = bounds.center - transform.position;
    }

    void Update()
    {
        // Har frame da world center ni qayta hisoblash (parent harakatlansa ham ishlaydi)
        Vector3 currentCenter = transform.position + visualCenterOffset;
        transform.RotateAround(currentCenter, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
