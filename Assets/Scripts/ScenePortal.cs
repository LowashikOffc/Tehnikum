using UnityEngine;
using UnityEngine.SceneManagement; // Обязательно для работы со сценами

public class ScenePortal : MonoBehaviour
{
    [Header("Настройки")]
    public string sceneName; // Название сцены, куда переходим (например, "Level2")
    public GameObject pressEText; // Ссылка на текст "Нажми E" (необязательно)

    private bool isPlayerInZone = false;

    void Start()
    {
        // Обычный код старта...
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // --- НОВЫЙ КОД: ПРОВЕРКА ВОЗВРАЩЕНИЯ ---
        if (GlobalData.isReturning)
        {
            // Если мы вернулись из мини-игры, телепортируемся на старое место
            transform.position = GlobalData.playerPosition;

            // (Важно для физики) Сбрасываем инерцию, чтобы не вылететь
            if (rb != null) rb.velocity = Vector3.zero;

            // Сбрасываем флаг, чтобы при перезапуске игры мы появлялись на старте
            GlobalData.isReturning = false;
        }
        // ----------------------------------------
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            // 1. Находим игрока (чтобы узнать его координаты)
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // 2. Сохраняем его позицию в нашу "Память"
                // (минус немного назад, чтобы не появиться внутри портала и не телепортироваться снова)
                GlobalData.playerPosition = player.transform.position - player.transform.forward * 2f;
                GlobalData.isReturning = true;
            }

            // 3. Загружаем мини-игру
            SceneManager.LoadScene(sceneName);
        }
    }

    // Когда кто-то входит в триггер
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Проверяем, что это Игрок
        {
            isPlayerInZone = true;
            Debug.Log("Можно нажать E");

            if (pressEText != null)
                pressEText.SetActive(true); // Показываем текст
        }
    }

    // Когда кто-то выходит из триггера
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;

            if (pressEText != null)
                pressEText.SetActive(false); // Скрываем текст
        }
    }
}