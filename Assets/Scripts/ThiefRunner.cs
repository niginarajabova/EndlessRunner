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

    [Header("Debug")]
    public bool showDebugLogs = true;

    private PlayerMovement player;
    private bool isGameStarted = false;

    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        if (showDebugLogs)
        {
            Debug.Log("[ThiefRunner] Initialized. Player found: " + (player != null) + " | LocalPos: " + transform.localPosition);
            if (player == null)
                Debug.LogWarning("[ThiefRunner] PlayerMovement NOT found! Donut won't follow.");
        }
    }

    void Update()
    {
        if (player == null) return;

        if (!isGameStarted && player.isGameStarted)
        {
            isGameStarted = true;

            if (showDebugLogs)
                Debug.Log("[ThiefRunner] Game started — Donut begins following player.");
        }

        if (!isGameStarted) return;

        float localZ = runDistanceForward;
        float targetLocalX = player.targetX - player.transform.position.x;
        float currentLocalX = Mathf.Lerp(transform.localPosition.x, targetLocalX, Time.deltaTime * followSmoothing);

        transform.localPosition = new Vector3(currentLocalX, transform.localPosition.y, localZ);
    }
}
