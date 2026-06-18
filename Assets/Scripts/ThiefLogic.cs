using UnityEngine;

/// <summary>
/// Ponchik hamrohning hovering animatsiyasi va particle effektlari.
/// ThiefRunner X/Z pozitsiyani boshqaradi, bu skript faqat Y (hover) va particle boshqaradi.
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
    public float baseHeight = 0.61f;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private PlayerMovement player;
    private bool initialized = false;

    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();

        baseHeight = transform.localPosition.y;
        initialized = true;

        if (showDebugLogs)
        {
            Debug.Log("[ThiefSpawner] Initialized. Player found: " + (player != null) + " | BaseHeight: " + baseHeight);
            Debug.Log("[ThiefSpawner] SugarParticles assigned: " + (sugarParticles != null));
        }
    }

    void Update()
    {
        if (!initialized || player == null) return;
        if (!player.isGameStarted) return;

        UpdateHover();
        UpdateParticles();
    }

    private void UpdateHover()
    {
        float hover = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        Vector3 pos = transform.localPosition;
        pos.y = baseHeight + hover;
        transform.localPosition = pos;
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
