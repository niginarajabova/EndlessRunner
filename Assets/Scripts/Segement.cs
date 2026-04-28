using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegementGenerator : MonoBehaviour
{
    public GameObject[] segment;
    private PlayerMovement playerScript; 
    
    [SerializeField] float zPos = 50f; 
    [SerializeField] bool creatingSegment = false;
    [SerializeField] float segmentLength = 50f; 
    
    void Start()
    {
        playerScript = GameObject.FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (playerScript == null || !playerScript.isGameStarted) return;

        if(creatingSegment == false && (zPos - playerScript.transform.position.z < 100))
        {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        }
    }
    
    IEnumerator SegmentGen()
    {
        int segmentNum = Random.Range(0, segment.Length);
        
        // 1. Создаем сегмент и записываем его в переменную 'nextSegment'
        GameObject nextSegment = Instantiate(segment[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
        
        // 2. ВОТ ТА САМАЯ ДЕТАЛЬ: говорим сегменту удалиться через 30 секунд
        // За это время игрок точно успеет его пробежать
        Destroy(nextSegment, 30f); 

        zPos += segmentLength;
        yield return new WaitForSeconds(0.1f);
        creatingSegment = false;
    }
}
