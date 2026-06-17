using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 
using System.Collections; 

public class PlayerMovement : MonoBehaviour
{
    [Header("Сюжет и Старт")]
    public bool isGameStarted = false; 
    public GameObject floatingTextPrefab;
    public GameObject startUI;       

    [Header("Бег и Рывки")]
    public float playerSpeed = 10f;
    private float targetSpeed;         
    public float maxSpeed = 30f;       
    public float acceleration = 0.1f;   
    public float sideSpeed = 30f; 
    public float laneDistance = 4f;
    public float targetX = 0f;

    [Header("Прыжок и Высота Земли")]
    public float jumpForce = 40f;
    public float gravity = -80f;     
    private float yVelocity = 0f;   
    public bool isGrounded;
    public float groundY = 2.0f;     // Эту высоту можно менять прямо в Инспекторе Unity!

    [Header("UI и Очки")]
    public int score = 0;
    public TextMeshProUGUI scoreText; 
    public GameObject collectEffectPrefab;

    void Start()
    {
        targetSpeed = playerSpeed;
        isGrounded = true;
        if (scoreText != null) scoreText.text = "Yummies: 0";
        
        // Автоматически ставим игрока на нужную высоту земли при старте
        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
    }

    void Update()
    {
        if (!isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGameStarted = true;
                if (startUI != null) startUI.SetActive(false);
            }
            return; 
        }

        if (targetSpeed < maxSpeed) targetSpeed += acceleration * Time.deltaTime;
        playerSpeed = Mathf.MoveTowards(playerSpeed, targetSpeed, 5f * Time.deltaTime);
        sideSpeed = 20f + (playerSpeed * 1.5f); 

        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            if (targetX > -laneDistance) targetX -= laneDistance;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            if (targetX < laneDistance) targetX += laneDistance;

        if (transform.position.y <= groundY + 0.01f) 
        {
            isGrounded = true;
            yVelocity = 0; 
            if (Input.GetKeyDown(KeyCode.Space)) 
            { 
                yVelocity = jumpForce; 
                isGrounded = false; 
            }
        }
        else
        {
            isGrounded = false;
            yVelocity += gravity * Time.deltaTime; 
        }

        float newX = Mathf.MoveTowards(transform.position.x, targetX, sideSpeed * Time.deltaTime);
        float newY = transform.position.y + (yVelocity * Time.deltaTime);
        if (newY < groundY) newY = groundY;
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

private void OnTriggerEnter(Collider other)
    {
        // ЭТА СТРОЧКА КРИЧИТ В КОНСОЛЬ ИМЯ ПРЕПЯТСТВИЯ:
        Debug.LogError("БЕЗУМНЫЙ ПЕРЕЗАПУСК! Игрок коснулся объекта: " + other.gameObject.name + " с тегом: " + other.tag);

        if (other.CompareTag("Obstacle")) 
        {
            playerSpeed = 0; 
            targetSpeed = 0;
            StartCoroutine(HitDelay()); 
        }
        else if (other.CompareTag("Slow")) 
{
    // Сироп просто временно снижает скорость и забирает вкусняшки
    playerSpeed = targetSpeed * 0.3f; // Замедляем до 30% от нормальной скорости
    score -= 2;
    if (score < 0) score = 0; 
    if (scoreText != null) scoreText.text = "Yummies: " + score;

    // ВНИМАНИЕ: Если здесь стоит StartCoroutine(HitDelay()); — СОТРИ ЕЁ! 
    // Эта строчка должна быть ТОЛЬКО в блоке "Obstacle" (для тигра).
}
        else if (other.CompareTag("Item"))
        {
            ItemData item = other.GetComponent<ItemData>();
            int pointsAdded = 1; // По умолчанию 1 балл

            if (item != null)
            {
                pointsAdded = item.scoreValue; // Берем индивидуальный балл конфеты
            }

            // =================================================================
            // ВОТ ОНА — СВЯЗЬ С ТВОИМ СЦЕНАРИЕМ ОЧКОВ:
            // Говорим СкорМенеджеру: "Эй, прибавь вот столько очков!"
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(pointsAdded);
            }
            // =================================================================

            // Спавним всплывающий текст +1, +5 и т.д. (если настроен префаб)
            if (floatingTextPrefab != null)
            {
                GameObject textObj = Instantiate(floatingTextPrefab, other.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                FloatingText floatingTextScript = textObj.GetComponent<FloatingText>();
                if (floatingTextScript != null)
                {
                    floatingTextScript.Setup(pointsAdded);
                }
            }

            // Эффекты искр и удаление конфеты с дороги
            if (collectEffectPrefab != null) Instantiate(collectEffectPrefab, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    IEnumerator HitDelay() 
    { 
        yield return new WaitForSeconds(0.1f); 
        GameOver(); 
    }

    void GameOver() 
    { 
        isGameStarted = false; 
        playerSpeed = 0; 
        Invoke("RestartLevel", 2f); 
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}