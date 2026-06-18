using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegementGenerator : MonoBehaviour
{
    [Header("Твои старые настройки сегментов")]
    public GameObject[] segment;
    [SerializeField] float zPos = 50f; 
    [SerializeField] bool creatingSegment = false;
    [SerializeField] float segmentLength = 50f; 
    
    [Header("Новые настройки наполнения (Тигра и Сладости)")]
    public GameObject[] sweetsPrefabs;   // Сюда в Инспекторе перетащи конфетки
    public GameObject[] obstaclePrefabs; // Сюда в Инспекторе перетащи тигра/заборы

    private PlayerMovement playerScript; 

    [Header("Debug")]
    [SerializeField] bool showDebugLogs = true;

    void Start()
    {
        playerScript = GameObject.FindObjectOfType<PlayerMovement>();

        if (showDebugLogs)
        {
            Debug.Log("[SegementGenerator] Initialized. Player found: " + (playerScript != null));
            Debug.Log("[SegementGenerator] Segments: " + segment.Length + " | Sweets: " + (sweetsPrefabs != null ? sweetsPrefabs.Length : 0) + " | Obstacles: " + (obstaclePrefabs != null ? obstaclePrefabs.Length : 0));
            Debug.Log("[SegementGenerator] StartZ: " + zPos + " | SegmentLength: " + segmentLength);
        }
    }

    void Update()
    {
        if (playerScript == null || !playerScript.isGameStarted) return;

        // Твоя старая проверка дистанции до игрока
        if (creatingSegment == false && (zPos - playerScript.transform.position.z < 100))
        {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        }
    }
    
    IEnumerator SegmentGen()
    {
        int segmentNum = Random.Range(0, segment.Length);
        GameObject nextSegment = Instantiate(segment[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
        
        Destroy(nextSegment, 30f); 

        if (showDebugLogs)
            Debug.Log("[SegementGenerator] New segment #" + segmentNum + " at Z: " + zPos);

        float[] lanes = new float[] { -4f, 0f, 4f };
        int obstacleLaneIndex = Random.Range(0, lanes.Length);

        if (showDebugLogs)
            Debug.Log("[SegementGenerator] Obstacle on lane index: " + obstacleLaneIndex + " (X: " + lanes[obstacleLaneIndex] + ")");

        for (int i = 0; i < lanes.Length; i++)
        {
            float laneX = lanes[i];

            if (i == obstacleLaneIndex)
            {
                if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
                {
                    int randomObstacle = Random.Range(0, obstaclePrefabs.Length);
                    float zOffset = Random.Range(5f, segmentLength - 5f);
                    GameObject obs = Instantiate(obstaclePrefabs[randomObstacle], new Vector3(laneX, 0f, zPos + zOffset), Quaternion.identity);
                    obs.transform.SetParent(nextSegment.transform);

                    if (showDebugLogs)
                        Debug.Log("[SegementGenerator] Obstacle: " + obstaclePrefabs[randomObstacle].name + " at (" + laneX + ", 0, " + (zPos + zOffset) + ")");
                }
            }
            else
            {
                if (sweetsPrefabs != null && sweetsPrefabs.Length > 0)
                {
                    int randomSweet = Random.Range(0, sweetsPrefabs.Length);
                    float zOffset = Random.Range(5f, segmentLength - 5f);
                    GameObject sweet = Instantiate(sweetsPrefabs[randomSweet], new Vector3(laneX, 2f, zPos + zOffset), Quaternion.identity);
                    sweet.transform.SetParent(nextSegment.transform);

                    if (showDebugLogs)
                        Debug.Log("[SegementGenerator] Sweet: " + sweetsPrefabs[randomSweet].name + " at (" + laneX + ", 2, " + (zPos + zOffset) + ")");
                }
            }
        }
        // Keyingi segment uchun pozitsiyani surish
        zPos += segmentLength;
        yield return new WaitForSeconds(0.1f);
        creatingSegment = false;
    }
}