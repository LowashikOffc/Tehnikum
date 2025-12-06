using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Нужен для смены сцен

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<WordData> wordDatabase;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText; // Ссылка на текст жизней

    private int score = 0;
    private int lives = 3; // Жизни

    // Впиши сюда ТОЧНОЕ имя твоей 3D сцены (как оно в папке Scenes)
    public string nameOf3DScene = "MainScene";

    void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public WordData GetRandomWord()
    {
        return wordDatabase[Random.Range(0, wordDatabase.Count)];
    }

    public void AddScore()
    {
        score++;
        UpdateUI();
    }

    // Новый метод: получение урона
    public void TakeDamage()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            EndMiniGame();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Счёт: " + score;
        if (livesText != null) livesText.text = "Жизни: " + lives;
    }

    void EndMiniGame()
    {
        Debug.Log("Игра окончена! Возврат в 3D мир...");

        // Загружаем 3D сцену
        SceneManager.LoadScene(nameOf3DScene);
    }
}