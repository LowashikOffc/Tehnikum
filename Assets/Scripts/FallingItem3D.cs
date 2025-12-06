using UnityEngine;

public class FallingItem3D : MonoBehaviour
{
    public string myWord;

    public void Setup(string word)
    {
        myWord = word;
        // Модель уже задана в префабе, менять спрайт не нужно
    }

    private void OnTriggerEnter(Collider other) // Заметил? Нет "2D"
    {
        if (other.CompareTag("Player"))
        {
            GameManager3D.Instance.CheckAnswer(myWord);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Удаляем, если упал слишком низко
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}