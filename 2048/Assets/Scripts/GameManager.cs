using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Board board;
    public CanvasGroup gameOver;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    private int score;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        hiscoreText.text = LoadHighScore().ToString();
        
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        board.ClearBoard();
        //Create 2 start tiles
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        audioSource.PlayOneShot(audioClip, 0.4f);
        board.enabled = false;
        gameOver.interactable = true;
        StartCoroutine(Fade(gameOver, 1f, 1f));
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

    public void IncreaseScore(int points){
        SetScore(score + points);
    }

    private void SetScore(int score){
        this.score = score;
        scoreText.text = score.ToString();
        SaveHighScore();
    }

    private void SaveHighScore(){
        int highScore = LoadHighScore();
        if(score > highScore){
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    private int LoadHighScore(){
        return PlayerPrefs.GetInt("HighScore", 0);
    }
}
