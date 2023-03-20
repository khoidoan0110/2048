using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Board board;
    public CanvasGroup gameOver;
    public static GameManager instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI hiscoreVictoryMenuText;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject victoryPanel;
    private int score, highScore;
    private bool settingsToggle = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        highScore = LoadHighScore();
        NewGame();
    }

    void Update()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            hiscoreText.text = score.ToString();
        }
    }

    public void NewGame()
    {
        AudioManager.instance.PlayMusic("Background");
        score = 0;
        scoreText.text = score.ToString();
        hiscoreText.text = LoadHighScore().ToString();

        gameOver.alpha = 0f;
        gameOver.interactable = false;

        victoryPanel.SetActive(false);

        board.ClearBoard();
        //Create 2 start tiles
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        AudioManager.instance.PlaySFX("Lose", 1f);
        board.enabled = false;
        gameOver.interactable = true;
        StartCoroutine(Fade(gameOver, 1f, 0.7f));
    }

    public void Victory()
    {
        AudioManager.instance.PlaySFX("Win", 1f);
        board.enabled = false;
        victoryPanel.SetActive(true);
        hiscoreVictoryMenuText.text = score.ToString();
    }

    public void ShowSettings()
    {
        settingsToggle = !settingsToggle;

        if (settingsToggle == true)
        {
            board.enabled = false;
            settingsPanel.SetActive(true);
        }
        else
        {
            board.enabled = true;
            settingsPanel.SetActive(false);
        }
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
}
