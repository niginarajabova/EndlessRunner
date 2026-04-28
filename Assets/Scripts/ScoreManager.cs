using UnityEngine;
using TMPro; // Используй это, если у тебя TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private TextMeshProUGUI scoreText; 
    private int score = 0;

    void Awake()
    {
        instance = this;
        scoreText = GetComponent<TextMeshProUGUI>(); // Сам находит текст на этом же объекте
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Yummies: " + score;
    }
}
