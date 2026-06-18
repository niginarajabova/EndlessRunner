using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Start")]
    public bool isGameStarted = false;
    public GameObject floatingTextPrefab;
    public GameObject startUI;

    [Header("Movement")]
    public float playerSpeed = 10f;
    private float targetSpeed;
    public float maxSpeed = 30f;
    public float acceleration = 0.1f;
    public float sideSpeed = 30f;
    public float laneDistance = 4f;
    public float targetX = 0f;
    private int currentLane = 0; // -1 chap, 0 markaz, 1 o'ng

    [Header("Jump")]
    public float jumpForce = 40f;
    public float gravity = -80f;
    private float yVelocity = 0f;
    public bool isGrounded;
    public float groundY = 2.0f;

    [Header("Effects")]
    public GameObject collectEffectPrefab;

    void Start()
    {
        targetSpeed = playerSpeed;
        isGrounded = true;
        currentLane = 0;
        targetX = 0f;
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

        UpdateSpeed();
        UpdateLaneInput();
        UpdateJump();
        UpdatePosition();
    }

    private void UpdateSpeed()
    {
        if (targetSpeed < maxSpeed)
            targetSpeed += acceleration * Time.deltaTime;

        playerSpeed = Mathf.MoveTowards(playerSpeed, targetSpeed, 5f * Time.deltaTime);
        sideSpeed = 20f + (playerSpeed * 1.5f);

        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);
    }

    private void UpdateLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentLane > -1)
            {
                currentLane--;
                targetX = currentLane * laneDistance;
            }
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentLane < 1)
            {
                currentLane++;
                targetX = currentLane * laneDistance;
            }
        }
    }

    private void UpdateJump()
    {
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
    }

    private void UpdatePosition()
    {
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

            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.SubtractScore(2);
            }
        }
        else if (other.CompareTag("Item"))
        {
            CollectItem(other.gameObject);
        }
    }

    private void CollectItem(GameObject itemObject)
    {
        ItemData item = itemObject.GetComponent<ItemData>();
        int pointsAdded = 1;

        if (item != null)
        {
            pointsAdded = item.scoreValue;
        }

        // ScoreManager orqali ball qo'shish
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(pointsAdded);
        }

        // Floating text effekti
        if (floatingTextPrefab != null)
        {
            GameObject textObj = Instantiate(floatingTextPrefab,
                itemObject.transform.position + Vector3.up * 0.5f, Quaternion.identity);
            FloatingText floatingTextScript = textObj.GetComponent<FloatingText>();
            if (floatingTextScript != null)
            {
                floatingTextScript.Setup(pointsAdded);
            }
        }

        // Yig'ish effekti
        if (item != null && item.collectEffect != null)
        {
            Renderer rend = itemObject.GetComponentInChildren<Renderer>();
            Vector3 spawnPos = (rend != null) ? rend.bounds.center : itemObject.transform.position;
            Instantiate(item.collectEffect, spawnPos, itemObject.transform.rotation);
        }
        else if (collectEffectPrefab != null)
        {
            Instantiate(collectEffectPrefab, itemObject.transform.position, Quaternion.identity);
        }

        Destroy(itemObject);
    }

    private IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GameOver();
    }

    private void GameOver()
    {
        isGameStarted = false;
        playerSpeed = 0;
        Invoke(nameof(RestartLevel), 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
