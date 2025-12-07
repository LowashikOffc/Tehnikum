using UnityEngine;
using System.Collections;

public class AssetSpawner : MonoBehaviour
{
    [Header("Префабы Объектов")]
    // Массивы префабов, которые должны иметь прикрепленный скрипт MovingObject
    public GameObject[] threatPrefabs;
    public GameObject[] assetPrefabs;

    [Header("Настройки Спауна")]
    public float spawnInterval = 1.5f; // Интервал между появлениями объектов
    public Transform spawnPoint;       // Точка, где появляются объекты

    void Start()
    {
        // Начинаем бесконечный цикл спауна
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        // Случайный выбор: Угроза (0) или Актив (1)
        bool isThreat = Random.Range(0, 2) == 0;
        GameObject objectToSpawn;

        if (isThreat)
        {
            // Выбираем случайный префаб из списка угроз
            objectToSpawn = threatPrefabs[Random.Range(0, threatPrefabs.Length)];
        }
        else
        {
            // Выбираем случайный префаб из списка активов
            objectToSpawn = assetPrefabs[Random.Range(0, assetPrefabs.Length)];
        }

        // Создаем объект в указанной точке
        GameObject spawned = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);

        // Устанавливаем правильный тип в MovingObject, если он не задан в префабе
        MovingObject movObj = spawned.GetComponent<MovingObject>();
        if (movObj != null)
        {
            movObj.type = isThreat ? ObjectType.Threat : ObjectType.Asset;
        }
    }
}