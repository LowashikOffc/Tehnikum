using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [Header("Настройки")]
    public string sceneName;        // Имя сцены мини-игры
    public GameObject pressEText;   // Текст "Нажмите Е"

    private bool isPlayerInZone = false;

    void Start()
    {
        // 1. Настройка курсора при старте сцены
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 2. Проверяем, вернулись ли мы из мини-игры
        if (GlobalData.isReturning)
        {
            // Нам нужно найти ИГРОКА, чтобы переместить его
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // ВАЖНО: Если на игроке есть CharacterController, его нужно отключить перед телепортом
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                // Телепортируем ИГРОКА (а не портал) на сохраненную позицию
                player.transform.position = GlobalData.playerPosition;
                player.transform.rotation = GlobalData.playerRotation; // Если сохраняли поворот

                // Включаем CharacterController обратно
                if (cc != null) cc.enabled = true;

                // Если используется Rigidbody, гасим инерцию
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null) rb.velocity = Vector3.zero;
            }

            // Сбрасываем флаг, чтобы при следующем запуске игры (не из мини-игры) телепорт не сработал
            GlobalData.isReturning = false;
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            SaveAndLoadMiniGame();
        }
    }

    void SaveAndLoadMiniGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // СОХРАНЕНИЕ ПОЗИЦИИ
            // Сохраняем позицию игрока + немного назад, чтобы он не появился прямо внутри триггера и не застрял
            GlobalData.playerPosition = player.transform.position - player.transform.forward * 1.5f;
            GlobalData.playerRotation = player.transform.rotation;

            // Ставим метку, что мы ушли в мини-игру
            GlobalData.isReturning = true;
        }

        // Загрузка сцены
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (pressEText != null) pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (pressEText != null) pressEText.SetActive(false);
        }
    }
}