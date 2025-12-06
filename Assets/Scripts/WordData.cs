using UnityEngine;

[System.Serializable] // Чтобы видеть список в Инспекторе
public class WordData
{
    public string text;      // Само слово
    public bool isCorrect;   // Правильно ли оно написано?
}