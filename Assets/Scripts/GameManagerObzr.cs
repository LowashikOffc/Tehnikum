using UnityEngine;
using TMPro; // Для работы с TextMeshPro

public class GameManagerObzr : MonoBehaviour
{
    [Header("UI Элементы")]
    public TextMeshProUGUI scoreTextUI;
    public TextMeshProUGUI healthTextUI;
    public GameObject gameOverPanel; // Панель, которая появится при проигрыше

    [Header("Игровые Параметры")]
    public int score = 0;
    public int health = 3;
    private bool isGameActive = true;

    void Start()
    {
        UpdateUI();
        gameOverPanel.SetActive(false); // Скрыть панель проигрыша в начале
    }

    void Update()
    {
        if (!isGameActive) return;

        // --- Обработка ввода игрока (клавиши) ---

        // Клавиша для "Угроза" (например, A или Левая стрелка)
        if (Input.GetKeyDown(KeyCode.A))
        {
            ProcessInput(ObjectType.Threat);
        }

        // Клавиша для "Безопасно / Актив" (например, D или Правая стрелка)
        if (Input.GetKeyDown(KeyCode.D))
        {
            ProcessInput(ObjectType.Asset);
        }
    }

    // Находит ближайший объект и вызывает его обработку
    void ProcessInput(ObjectType action)
    {
        // Ищем объект, который находится ближе всего к шлюзу
        MovingObject[] movingObjects = FindObjectsOfType<MovingObject>();
        MovingObject target = null;
        float minY = float.MaxValue; // Для поиска ближайшего сверху

        foreach (MovingObject obj in movingObjects)
        {
            if (obj.transform.position.y < minY)
            {
                minY = obj.transform.position.y;
                target = obj;
            }
        }

        if (target != null)
        {
            target.HandlePlayerInput(action);
        }
    }

    // --- Обработка результатов от MovingObject ---

    public void CorrectAction(MovingObject obj)
    {
        score += 10;
        // Можно добавить анимацию "Правильно"
        UpdateUI();
    }

    public void WrongAction(MovingObject obj)
    {
        health -= 1;
        score -= 5; // Штраф
        // Можно добавить анимацию "Неправильно"
        UpdateUI();
        CheckGameOver();
    }

    public void MissedAction(MovingObject obj)
    {
        health -= 1;
        // Можно добавить анимацию "Пропущено"
        UpdateUI();
        CheckGameOver();
    }

    // --- Вспомогательные функции ---

    void UpdateUI()
    {
        scoreTextUI.text = "Счет: " + score;
        healthTextUI.text = "Жизни: " + health;
    }

    void CheckGameOver()
    {
        if (health <= 0)
        {
            isGameActive = false;
            gameOverPanel.SetActive(true);
            // Остановить спаун объектов
            AssetSpawner spawner = FindObjectOfType<AssetSpawner>();
            if (spawner != null)
            {
                spawner.StopAllCoroutines();
            }
            Debug.Log("Игра окончена! Итоговый счет: " + score);
        }
    }
}