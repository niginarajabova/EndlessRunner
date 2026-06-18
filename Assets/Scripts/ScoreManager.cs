using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private TextMeshProUGUI scoreText;
    private int score = 0;

    public int Score => score;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.SetText("Yummies: {0}", score);
        }
    }
}
