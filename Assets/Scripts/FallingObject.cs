using UnityEngine;

public class FallingObject : MonoBehaviour
{
    // Время жизни, чтобы удалять объекты, вышедшие за пределы экрана
    public float lifetime = 10f;

    void Start()
    {
        // Убеждаемся, что у падающего объекта есть тег "Danger"
        gameObject.tag = "Danger";

        // Удаляем объект через 10 секунд, чтобы не засорять память
        Destroy(gameObject, lifetime);
    }
}