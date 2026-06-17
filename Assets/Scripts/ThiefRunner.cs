using UnityEngine;

public class ThiefRunner : MonoBehaviour
{
    [Header("Настройки позиции Пончика")]
    [Tooltip("На сколько метров ПОДНЯТЬ Пончика вверх")]
    public float flyHeight = 3f;        

    [Tooltip("На сколько метров ОТОДВИНУТЬ Пончика вперёд от игрока")]
    public float runDistanceForward = 3f; 

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
            // Рассчитываем позицию: летит впереди на runDistanceForward и ВЫСОКО на flyHeight
            float targetZ = player.transform.position.z + runDistanceForward;
            float targetX = Mathf.Lerp(transform.position.x, player.targetX, Time.deltaTime * 3f);
            
            // ИСПРАВЛЕНО: Вместо 0f теперь подставляется flyHeight!
            transform.position = new Vector3(targetX, flyHeight, targetZ);
        }
    }
}