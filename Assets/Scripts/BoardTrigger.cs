// BoardTrigger.cs - Прикрепи к доске/триггеру

using UnityEngine;

[RequireComponent(typeof(Collider))] // Требует коллайдер
public class BoardTrigger : MonoBehaviour
{
    // Ссылка на основной скрипт игры
    public WordGameManager gameManager;

    // Имя тега игрока (обычно "Player")
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошел именно игрок
        if (other.CompareTag(playerTag))
        {
            // Здесь можно показать подсказку типа "Нажмите E для взаимодействия"
            Debug.Log("Игрок рядом с доской. Нажмите E для игры.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag) && Input.GetKeyDown(KeyCode.E))
        {
            // Начинаем игру при нажатии E
            if (gameManager != null)
            {
                gameManager.StartGame();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Убираем подсказку
            Debug.Log("Игрок отошел от доски.");
        }
    }
}