using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 
using System.Collections; 

public class PlayerMovement : MonoBehaviour
{
    [Header("Сюжет и Старт")]
    public bool isGameStarted = false; 
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
        if (other.CompareTag("Obstacle")) 
        {
            playerSpeed = 0; 
            targetSpeed = 0;
            StartCoroutine(HitDelay()); 
        }
        else if (other.CompareTag("Slow")) 
        {
            playerSpeed = targetSpeed * 0.3f; 
            score -= 2;
            if (score < 0) score = 0; 
            if (scoreText != null) scoreText.text = "Yummies: " + score;
        }
        else if (other.CompareTag("Item"))
        {
            ItemData item = other.GetComponent<ItemData>();
            
            if (item != null)
            {
                score += item.scoreValue;
            }
            else
            {
                score += 1;
            }

            if (scoreText != null) scoreText.text = "Yummies: " + score;
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