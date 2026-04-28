using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 
using System.Collections; 

public class PlayerMovement : MonoBehaviour
{
    [Header("Сюжет и Старт")]
    public bool isGameStarted = false; 
    public GameObject thief;         
    public GameObject startUI;       
    public ParticleSystem sugarParticles; 

    [Header("Рассыпание сладостей")]
    public GameObject[] itemsToDrop;  
    public float dropInterval = 1f;   
    private float dropTimer;

    [Header("Бег и Рывки")]
    public float playerSpeed = 10f;
    private float targetSpeed;         
    public float maxSpeed = 30f;       
    public float acceleration = 0.1f;  
    public float sideSpeed = 30f; 
    public float laneDistance = 4f;
    private float targetX = 0f;

    [Header("Прыжок")]
    public float jumpForce = 40f;
    public float gravity = -80f;     
    private float yVelocity = 0f;   
    private bool isGrounded;
    public float groundY = 1.0f;     

    [Header("UI и Очки")]
    public int score = 0;
    public TextMeshProUGUI scoreText; 
    public GameObject collectEffectPrefab;

    private float startThiefZ;
    private float startThiefRotY;
    private Vector3 startThiefScale; 

    void Start()
    {
        targetSpeed = playerSpeed;
        if (thief != null)
        {
            startThiefZ = thief.transform.localPosition.z;
            startThiefRotY = thief.transform.localEulerAngles.y;
            startThiefScale = thief.transform.localScale; 
            if(thief.GetComponent<Rigidbody>()) thief.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (sugarParticles != null) sugarParticles.Stop();
    }

    void Update()
    {
        if (!isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGameStarted = true;
                if (startUI != null) startUI.SetActive(false);
                if (sugarParticles != null) sugarParticles.Play();
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
            if (Input.GetKeyDown(KeyCode.Space)) { yVelocity = jumpForce; isGrounded = false; }
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

        if (thief != null)
        {
            float hover = Mathf.Sin(Time.time * 3f) * 0.3f; 
            float dynamicZ = startThiefZ + Mathf.Sin(Time.time * 0.7f) * 2.5f;
            float thiefX = Mathf.Lerp(thief.transform.localPosition.x, targetX, Time.deltaTime * 5f);
            thief.transform.localPosition = new Vector3(thiefX, 2.5f + hover, dynamicZ);
            thief.transform.localScale = startThiefScale; 

            if (!isGrounded) thief.transform.Rotate(Vector3.right, 500f * Time.deltaTime);
            else
            {
                float tilt = (targetX - thief.transform.localPosition.x) * 15f;
                Quaternion targetRot = Quaternion.Euler(0, startThiefRotY, tilt);
                thief.transform.localRotation = Quaternion.Lerp(thief.transform.localRotation, targetRot, Time.deltaTime * 10f);
            }

            // --- БЛОК СПАВНА СЛАДОСТЕЙ ---
            dropTimer -= Time.deltaTime;
            if (dropTimer <= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(thief.transform.position, Vector3.down, out hit, 10f))
                {
                    if (!hit.collider.CompareTag("Obstacle") && !hit.collider.CompareTag("Slow") && itemsToDrop.Length > 0)
                    {
                        float dropX = 0;
                        if (thief.transform.position.x < -laneDistance / 2f) dropX = -laneDistance;
                        else if (thief.transform.position.x > laneDistance / 2f) dropX = laneDistance;

                        int randomIndex = Random.Range(0, itemsToDrop.Length);
                        
                        // ФИКС ВЫСОТЫ: поднимаем предмет на 1.5 метра над полом
                        float safeY = groundY + 0.5f; 
                        
                        GameObject newItem = Instantiate(itemsToDrop[randomIndex], new Vector3(dropX, safeY, thief.transform.position.z), Quaternion.identity);
                        Destroy(newItem, 15f);
                    }
                }
                dropTimer = dropInterval;
            }
            
            if (sugarParticles != null)
            {
                var emission = sugarParticles.emission;
                emission.rateOverTime = playerSpeed * 2f;
            }
        }
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
            score += (item != null) ? item.scoreValue : 1;
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
        if (sugarParticles != null) sugarParticles.Stop(); 
        Invoke("RestartLevel", 2f); 
    }

    void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
