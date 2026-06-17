using UnityEngine;

public class ThiefRunner : MonoBehaviour
{
    [Header("Настройки дистанции")]
    public float runDistanceBack = 50f; // Дистанция впереди игрока

    private bool isGameStarted = false;

    void Start()
    {
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player == null) return;

        if (!isGameStarted && player.isGameStarted)
        {
            isGameStarted = true;
        }

        if (isGameStarted)
        {
            // Пончик просто летит впереди игрока на высоте дороги (Y = 0)
            float targetZ = player.transform.position.z + runDistanceBack;
            float targetX = Mathf.Lerp(transform.position.x, player.targetX, Time.deltaTime * 5f);
            
            transform.position = new Vector3(targetX, 0f, targetZ);
        }
    }
}