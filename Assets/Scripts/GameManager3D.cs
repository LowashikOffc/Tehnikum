using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Для перехода между сценами

public class GameManager3D : MonoBehaviour
{
    public static GameManager3D Instance;

    [Header("Настройки")]
    public WordData3D[] words; // Теперь он ссылается на новый класс
    public float spawnRate = 2f;
    public float spawnHeight = 8f; // Высота, откуда падают предметы
    public float spawnRangeX = 6f; // Разброс по ширине

    [Header("Жизни и Сцены")]
    public int maxLives = 3;
    public string mainSceneName = "MainWorld"; // Имя твоей главной 3D сцены

    [Header("UI")]
    public TMP_Text targetWordText;
    public TMP_Text scoreText;
    public TMP_Text livesText;

    private string currentTargetWord;
    private int score = 0;
    private int currentLives;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        currentLives = maxLives;
        UpdateUI();
        PickNewWord();
        StartCoroutine(SpawnItemsRoutine());
    }

    void PickNewWord()
    {
        int randomIndex = Random.Range(0, words.Length);
        currentTargetWord = words[randomIndex].englishWord;
        targetWordText.text = "Find: " + currentTargetWord;
    }

    public void CheckAnswer(string caughtWord)
    {
        if (caughtWord == currentTargetWord)
        {
            score += 10;
            PickNewWord();
        }
        else
        {
            currentLives--;
            if (currentLives <= 0)
            {
                EndGame();
            }
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + currentLives;
    }

    void EndGame()
    {
        Debug.Log("Game Over! Teleporting...");
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator SpawnItemsRoutine()
    {
        while (currentLives > 0)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        // 1. Выбираем случайные данные
        int randomIndex = Random.Range(0, words.Length);
        WordData3D data = words[randomIndex];

        // 2. Выбираем точку спавна
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnHeight, 0); // Z=0, чтобы падали в одной плоскости с игроком

        // 3. Создаем префаб из WordData
        GameObject newItem = Instantiate(data.modelPrefab, spawnPos, Quaternion.identity);

        // 4. Настраиваем логику
        // Важно: на префабе должен висеть скрипт FallingItem3D
        newItem.GetComponent<FallingItem3D>().Setup(data.englishWord);
    }
}