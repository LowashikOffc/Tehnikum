using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private float xLimit = 8f; // Границы экрана (подстрой под себя)

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Стрелки или A/D

        Vector3 newPos = transform.position + Vector3.right * moveInput * speed * Time.deltaTime;

        // Ограничение, чтобы не уехать за экран
        newPos.x = Mathf.Clamp(newPos.x, -xLimit, xLimit);

        transform.position = newPos;
    }
}