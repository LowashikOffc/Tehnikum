using UnityEngine;

// Определяем тип объекта
public enum ObjectType { Threat, Asset }

public class MovingObject : MonoBehaviour
{
    // Тип этого конкретного объекта
    public ObjectType type;

    // Скорость движения объекта
    public float moveSpeed = 5f;

    // Ссылка на GameManager
    private GameManagerObzr gameManager;

    void Start()
    {
        // Ищем GameManager на сцене
        gameManager = FindObjectOfType<GameManagerObzr>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager не найден на сцене!");
        }
    }

    void Update()
    {
        // Движение объекта (например, вниз)
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    // Вызывается из GameManager, когда игрок нажимает кнопку
    public void HandlePlayerInput(ObjectType action)
    {
        // Проверяем, соответствует ли действие игрока типу объекта
        if (action == type)
        {
            // Правильно: Отправляем положительный результат
            gameManager.CorrectAction(this);
        }
        else
        {
            // Неправильно: Отправляем отрицательный результат
            gameManager.WrongAction(this);
        }
        // Уничтожаем объект после обработки
        Destroy(gameObject);
    }

    // Если объект достиг конца (пропущен игроком)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gate")) // Убедитесь, что у вашего "шлюза" есть тег "Gate"
        {
            // Объект пропущен - это всегда ошибка, так как игрок не принял решение
            gameManager.MissedAction(this);
            Destroy(gameObject);
        }
    }
}