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

    private PlayerMovement player;
    private bool initialized = false;

    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();

        // Boshlang'ich local Y ni base height sifatida saqlash
        baseHeight = transform.localPosition.y;
        initialized = true;
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
        // Faqat Y ni boshqarish — X va Z ni ThiefRunner boshqaradi
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
