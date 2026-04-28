using UnityEngine;

public class ItemData : MonoBehaviour 
{ 
    public int scoreValue = 3;       
    public GameObject collectEffect; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        // 1. Начисляем очки через ScoreManager
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        // 2. Создаем визуальный эффект
        if (collectEffect != null)
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            Vector3 spawnPos = (rend != null) ? rend.bounds.center : transform.position;
            Instantiate(collectEffect, spawnPos, transform.rotation);
        }

        Debug.Log("Собрано! + " + scoreValue);

        // 3. Удаляем предмет
        Destroy(gameObject);
    }
}
