using UnityEngine;
using TMPro;

public class FallingWord : MonoBehaviour
{
    public float fallSpeed = 3f;
    private bool isCorrectWord;
    private TextMeshPro textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(WordData data)
    {
        if (textMesh != null) textMesh.text = data.text;
        isCorrectWord = data.isCorrect;
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Если слово упало вниз
        if (transform.position.y < -6f)
        {
            // Если пропустили ПРАВИЛЬНОЕ слово — теряем жизнь
            if (isCorrectWord)
            {
                GameManager.Instance.TakeDamage();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                if (isCorrectWord)
                {
                    GameManager.Instance.AddScore(); // Поймали хорошее
                }
                else
                {
                    GameManager.Instance.TakeDamage(); // Поймали плохое — теряем жизнь
                }
            }
            Destroy(gameObject);
        }
    }
}