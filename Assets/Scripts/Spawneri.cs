using UnityEngine;
using System.Collections;

public class Spawneri : MonoBehaviour
{
    public GameObject fallingObjectPrefab;
    public float spawnRate = 1.5f; // Интервал между спауном
    public float spawnRangeX = 8f; // Максимальное отклонение по горизонтали от центра

    void Start()
    {
        // Запускаем корутину для постоянного спауна
        StartCoroutine(SpawnObjectsRoutine());
    }

    IEnumerator SpawnObjectsRoutine()
    {
        while (true) // Бесконечный цикл, пока игра работает
        {
            yield return new WaitForSeconds(spawnRate); // Ждем заданное время

            SpawnObject();

            // Опционально: Ускоряем игру со временем, уменьшая интервал спауна
            spawnRate = Mathf.Max(0.5f, spawnRate - 0.01f);
        }
    }

    void SpawnObject()
    {
        // Генерируем случайную позицию X в пределах диапазона
        float randomX = Random.Range(-spawnRangeX / 2, spawnRangeX / 2);

        // Определяем позицию спауна
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0f);

        // Создаем новый объект
        Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);
    }
}