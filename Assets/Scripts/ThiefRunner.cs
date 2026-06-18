using UnityEngine;

public class ThiefRunner : MonoBehaviour
{
    [Header("Ponchik pozitsiyasi")]
    [Tooltip("Ponchikni o'yinchidan qancha metr oldinga surish")]
    public float runDistanceForward = 3.8f;

    [Tooltip("Yon harakatni qanchalik tez ergashishi")]
    public float followSmoothing = 3f;

    [Tooltip("Ponchik uchish balandligi (local Y)")]
    public float flyHeight = 3.8f;

    private PlayerMovement player;
    private bool isGameStarted = false;

    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    void Update()
    {
        if (player == null) return;

        if (!isGameStarted && player.isGameStarted)
        {
            isGameStarted = true;
        }

        if (!isGameStarted) return;

        // DoughnutThief Player ning CHILD obyekti — shuning uchun LOCAL position ishlatamiz
        // Parent (Player) allaqachon Z bo'ylab harakatlanadi, biz faqat offset beramiz
        float localZ = runDistanceForward;

        // X pozitsiya: Player ning targetX ni local ga aylantirish
        // Parent position.x = player hozirgi X, targetX = mo'ljallangan X
        // Local X = targetX - parent.position.x (ya'ni parent ga nisbatan qancha offset)
        float targetLocalX = player.targetX - player.transform.position.x;
        float currentLocalX = Mathf.Lerp(transform.localPosition.x, targetLocalX, Time.deltaTime * followSmoothing);

        // Y ni ThiefSpawner boshqaradi, biz faqat X va Z ni o'rnatamiz
        transform.localPosition = new Vector3(currentLocalX, transform.localPosition.y, localZ);
    }
}
