using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Color textColor = Color.white;
    [SerializeField] int fontSize = 72;
    [SerializeField] bool addShadow = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Apply visual settings to score text
        if (scoreText != null)
        {
            scoreText.color = textColor;
            scoreText.fontSize = fontSize;
            scoreText.fontStyle = FontStyles.Bold;
            
            // Add shadow effect if enabled
            if (addShadow)
            {
                AddShadowEffect();
            }
            
            // Initial score display
            UpdateScoreDisplay();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
        StartCoroutine(ScorePopEffect()); // Add pop animation
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score.ToString();
        }
    }

    void AddShadowEffect()
    {
        // You can manually add a shadow component in the Inspector
        // Or we can add one programmatically
        var shadow = scoreText.GetComponent<UnityEngine.UI.Shadow>();
        if (shadow == null)
        {
            shadow = scoreText.gameObject.AddComponent<UnityEngine.UI.Shadow>();
            shadow.effectColor = Color.black;
            shadow.effectDistance = new Vector2(2, -2);
        }
    }

    IEnumerator ScorePopEffect()
    {
        if (scoreText != null)
        {
            // Store original size
            float originalSize = scoreText.fontSize;
            
            // Pop effect - scale up
            scoreText.fontSize = originalSize * 1.3f;
            scoreText.color = Color.yellow; // Flash yellow
            
            yield return new WaitForSeconds(0.1f);
            
            // Return to normal
            scoreText.fontSize = originalSize;
            scoreText.color = textColor;
        }
    }
}