using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    public float speed = 10f;
    public float xLimit = 7f; // Ограничение движения по бокам

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // Двигаем в 3D пространстве
        transform.Translate(Vector3.right * moveInput * speed * Time.deltaTime);

        // Ограничиваем позицию
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        transform.position = pos;
    }
}