using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private TextMeshProUGUI scoreText;
    private int score = 0;

    public int Score => score;

    [Header("Debug")]
    public bool showDebugLogs = true;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            if (showDebugLogs)
                Debug.LogWarning("[ScoreManager] Duplicate instance destroyed: " + gameObject.name);

            Destroy(gameObject);
            return;
        }
        instance = this;
        scoreText = GetComponent<TextMeshProUGUI>();

        if (showDebugLogs)
        {
            Debug.Log("[ScoreManager] Initialized. TextMeshPro found: " + (scoreText != null));
            if (scoreText == null)
                Debug.LogWarning("[ScoreManager] TextMeshProUGUI component NOT found on " + gameObject.name + "! UI won't update.");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        if (showDebugLogs)
            Debug.Log("[ScoreManager] +(" + amount + ") → Total: " + score);

        UpdateUI();
    }

    public void SubtractScore(int amount)
    {
        int oldScore = score;
        score -= amount;
        if (score < 0) score = 0;

        if (showDebugLogs)
            Debug.Log("[ScoreManager] -(" + amount + ") → " + oldScore + " → " + score);

        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;

        if (showDebugLogs)
            Debug.Log("[ScoreManager] Score RESET to 0");

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
