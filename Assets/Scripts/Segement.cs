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

    void Start()
    {
        playerScript = GameObject.FindObjectOfType<PlayerMovement>();
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
        // 1. ТВОЙ СТАРЫЙ КОД: Выбираем и создаем сегмент дороги на высоте 0
        int segmentNum = Random.Range(0, segment.Length);
        GameObject nextSegment = Instantiate(segment[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
        
        // 2. ТВОЙ СТАРЫЙ КОД: Удаляем сегмент через 30 секунд
        Destroy(nextSegment, 30f); 

        // =================================================================
        // НОВАЯ МАГИЯ: Спавним объекты внутри созданного сегмента
        // Распределяем по трем полосам: левая (-4), центр (0), правая (4)
        float[] lanes = new float[] { -4f, 0f, 4f };

        // Случайно выбираем полосу, на которой будет стоять Опасность (Тигр)
        int obstacleLaneIndex = Random.Range(0, lanes.Length);

        for (int i = 0; i < lanes.Length; i++)
        {
            float laneX = lanes[i];

            if (i == obstacleLaneIndex)
            {
                // Если это полоса для тигра — спавним ПРЕПЯТСТВИЕ
                if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
                {
                    int randomObstacle = Random.Range(0, obstaclePrefabs.Length);
                    // Спавним на высоте дороги (Y=0) со случайным небольшим сдвигом вперед/назад
                    GameObject obs = Instantiate(obstaclePrefabs[randomObstacle], new Vector3(laneX, 0f, zPos + Random.Range(-10f, 10f)), Quaternion.identity);
                    
                    // Чтобы тигр удалялся ВМЕСТЕ с дорогой, делаем его "ребенком" этого сегмента
                    obs.transform.SetParent(nextSegment.transform);
                }
            }
            else
            {
                // На остальных двух полосах спавним СЛАДОСТИ (Тут тигра точно не будет!)
                if (sweetsPrefabs != null && sweetsPrefabs.Length > 0)
                {
                    int randomSweet = Random.Range(0, sweetsPrefabs.Length);
                    // Приподнимаем сладость на Y=0.5f, чтобы она красиво висела над землей
                    GameObject sweet = Instantiate(sweetsPrefabs[randomSweet], new Vector3(laneX, 0.5f, zPos + Random.Range(-15f, 15f)), Quaternion.identity);
                    
                    // Привязываем сладость к куску дороги, чтобы она тоже удалилась через 30 сек
                    sweet.transform.SetParent(nextSegment.transform);
                }
            }
        }
        // =================================================================

        // 3. ТВОЙ СТАРЫЙ КОД: Сдвигаем позицию для следующего куска
        zPos += segmentLength;
        yield return new WaitForSeconds(0.1f);
        creatingSegment = false;
    }
}