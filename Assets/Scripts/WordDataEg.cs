// WordDataEg.cs
using UnityEngine;
using System.Collections.Generic;

// Имя класса изменено на WordDataEg!
[System.Serializable]
public class WordDataEg
{
    // Английское слово, которое нужно перевести
    public string englishWord;

    // Правильный русский перевод
    public string russianTranslation;

    // Список из 3-4 вариантов ответа (включая правильный)
    public List<string> answerOptions;
}