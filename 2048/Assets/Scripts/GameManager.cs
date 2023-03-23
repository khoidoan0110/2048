using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        scoreText.text = score.ToString();
        hiscoreVictoryMenuText.text = score.ToString();
        
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
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
        AudioManager.instance.StopMusic();
        board.enabled = false;
        gameOver.interactable = true;
        StartCoroutine(Fade(gameOver, 1f, 0.7f));
    }

    public void Victory()
    {
        AudioManager.instance.PlaySFX("Win", 1f);
        AudioManager.instance.StopMusic();
        board.enabled = false;
        victoryPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        board.enabled = false;
        settingsPanel.SetActive(true);
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
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("highScore", 0);
    }

    public void CloseSettings()
    {
        board.enabled = true;
        settingsPanel.SetActive(false);
    }

    public void Home()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
