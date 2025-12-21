using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Получаем ввод по горизонтали (-1, 0, 1)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Создаем вектор движения только по X (и Z=0, так как 2D)
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f);

        // Применяем скорость (используем Time.deltaTime для плавности и Frame-независимости)
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    // Обработка столкновений
    void OnCollisionEnter(Collision collision)
    {
        // Проверяем тег "Danger"
        if (collision.gameObject.CompareTag("Danger"))
        {
            Debug.Log("GAME OVER! Вы столкнулись с опасностью.");
            // Останавливаем игру или перезагружаем сцену
            Time.timeScale = 0f; // Останавливает все физические и временные процессы
            // TODO: Добавить UI для перезапуска
        }
    }
}