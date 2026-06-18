using UnityEngine;

/// <summary>
/// Ponchik hamrohning hovering animatsiyasi, tilting va particle effektlari.
/// Bu skript ThiefRunner bilan birga ishlaydi — ThiefRunner pozitsiyani boshqaradi,
/// ThiefSpawner esa vizual effektlarni (hover, tilt, particles) boshqaradi.
/// ESLATMA: Bu skript hozircha to'liq realize qilinmagan — itemsToDrop logikasi yo'q.
/// </summary>
public class ThiefSpawner : MonoBehaviour
{
    [Header("Shirinliklarni tashlash (hozircha ishlatilmaydi)")]
    public GameObject[] itemsToDrop;

    [Header("Particle effekti")]
    public ParticleSystem sugarParticles;

    [Header("Hovering sozlamalari")]
    public float hoverSpeed = 3f;
    public float hoverAmplitude = 0.3f;
    public float tiltMultiplier = 15f;

    private PlayerMovement player;
    private float startRotY;
    private Vector3 baseLocalPosition;
    private bool initialized = false;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        startRotY = transform.localEulerAngles.y;
        baseLocalPosition = transform.localPosition;
        initialized = true;
    }

    void Update()
    {
        if (!initialized || player == null) return;
        if (!player.isGameStarted) return;

        UpdateHover();
        UpdateTilt();
        UpdateParticles();
    }

    private void UpdateHover()
    {
        float hover = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        Vector3 pos = transform.localPosition;
        pos.y = baseLocalPosition.y + hover;
        transform.localPosition = pos;
    }

    private void UpdateTilt()
    {
        // Rotatsiyani boshlang'ich holatda ushlab turish (qiyshaymaslik uchun)
        Quaternion targetRot = Quaternion.Euler(0, startRotY, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * 10f);
    }

    private void UpdateParticles()
    {
        if (sugarParticles != null)
        {
            var emission = sugarParticles.emission;
            emission.rateOverTime = player.playerSpeed * 2f;
        }
    }
}
