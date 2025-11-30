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
        // Скрываем подсказку в начале
        if (pressEText != null)
            pressEText.SetActive(false);
    }

    void Update()
    {
        // Если игрок в зоне И нажал E
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            // Загружаем сцену
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