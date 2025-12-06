using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject wordPrefab;
    public float spawnRate = 2f;
    private float nextSpawnTime;
    public float spawnXLimit = 7f; // Ширина спавна

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnWord();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnWord()
    {
        float randomX = Random.Range(-spawnXLimit, spawnXLimit);
        Vector3 spawnPos = new Vector3(randomX, 6f, 0); // 6f - высота спавна

        // Важный момент: Если используешь UI Text, спавнить нужно внутри Canvas.
        // Для простоты примера, предположим, что TextMeshPro у нас World Space (не UI), 
        // либо мы инстанцируем его как обычный объект.

        GameObject wordObj = Instantiate(wordPrefab, spawnPos, Quaternion.identity);

        // Получаем данные
        WordData data = GameManager.Instance.GetRandomWord();
        wordObj.GetComponent<FallingWord>().Setup(data);
    }
}