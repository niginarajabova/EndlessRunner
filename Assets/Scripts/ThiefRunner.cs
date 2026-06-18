using UnityEngine;

public class ThiefRunner : MonoBehaviour
{
    [Header("Ponchik pozitsiyasi")]
    [Tooltip("Ponchikni qancha metr yuqoriga ko'tarish")]
    public float flyHeight = 3f;

    [Tooltip("Ponchikni o'yinchidan qancha metr oldinga surish")]
    public float runDistanceForward = 3f;

    [Tooltip("Yon harakatni qanchalik tez ergashishi")]
    public float followSmoothing = 3f;

    private PlayerMovement player;
    private bool isGameStarted = false;

    void Start()
    {
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

        if (isGameStarted)
        {
            float targetZ = player.transform.position.z + runDistanceForward;
            float targetXPos = Mathf.Lerp(transform.position.x, player.targetX, Time.deltaTime * followSmoothing);

            transform.position = new Vector3(targetXPos, flyHeight, targetZ);
        }
    }
}
