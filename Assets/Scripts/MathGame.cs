using UnityEngine;
using TMPro; // Библиотека для красивого текста
using UnityEngine.UI; // Библиотека для интерфейса
using System.Collections;
using System.Collections.Generic;

public class MathGame : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button[] answerButtons;

    [Header("Настройки")]
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 0.1f); // Прозрачный белый
    [SerializeField] private Color correctColor = new Color(0f, 1f, 0.2f, 1f); // Неоновый зеленый
    [SerializeField] private Color wrongColor = new Color(1f, 0f, 0.3f, 1f);   // Неоновый красный

    private int correctAnswer;
    private int currentScore = 0;
    private bool canClick = true;

    void Start()
    {
        currentScore = 0;
        UpdateScore();
        GenerateQuestion();
    }

    void GenerateQuestion()
    {
        canClick = true;

        // Сброс цветов кнопок
        foreach (var btn in answerButtons)
        {
            btn.image.color = normalColor;
            btn.interactable = true;
        }

        // 1. Генерация чисел
        int a = Random.Range(1, 20);
        int b = Random.Range(1, 20);

        // 2. Случайная операция (0: +, 1: -)
        int op = Random.Range(0, 2);
        string operationSymbol = "";

        if (op == 0)
        {
            correctAnswer = a + b;
            operationSymbol = "+";
        }
        else
        {
            // Чтобы не было отрицательных чисел для простоты
            if (a < b) { int temp = a; a = b; b = temp; }
            correctAnswer = a - b;
            operationSymbol = "-";
        }

        // 3. Красивый вывод вопроса
        questionText.text = $"{a} {operationSymbol} {b} = ?";

        // 4. Генерация вариантов ответов
        List<int> answers = new List<int>();
        answers.Add(correctAnswer);

        while (answers.Count < answerButtons.Length)
        {
            int wrong = correctAnswer + Random.Range(-5, 6);
            if (wrong != correctAnswer && !answers.Contains(wrong) && wrong >= 0)
            {
                answers.Add(wrong);
            }
        }

        // Перемешиваем ответы (алгоритм Фишера-Йетса)
        for (int i = 0; i < answers.Count; i++)
        {
            int temp = answers[i];
            int randomIndex = Random.Range(i, answers.Count);
            answers[i] = answers[randomIndex];
            answers[randomIndex] = temp;
        }

        // 5. Назначение текста кнопкам
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Получаем текст внутри кнопки
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i].ToString();

            int capturedValue = answers[i]; // Захват переменной для лямбды
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(capturedValue, i));
        }
    }

    void OnAnswerSelected(int selectedValue, int buttonIndex)
    {
        if (!canClick) return;
        canClick = false;

        if (selectedValue == correctAnswer)
        {
            // Правильно!
            answerButtons[buttonIndex].image.color = correctColor;
            currentScore++;
            StartCoroutine(WaitAndRestart(1f)); // Ждем 1 секунду
        }
        else
        {
            // Ошибка!
            answerButtons[buttonIndex].image.color = wrongColor;

            // Подсветим правильную кнопку, чтобы игрок знал ответ
            foreach (var btn in answerButtons)
            {
                if (btn.GetComponentInChildren<TextMeshProUGUI>().text == correctAnswer.ToString())
                {
                    btn.image.color = correctColor;
                }
            }

            currentScore = 0; // Обнуляем счет при ошибке (хардкор!)
            StartCoroutine(WaitAndRestart(1.5f));
        }
        UpdateScore();
    }

    IEnumerator WaitAndRestart(float delay)
    {
        yield return new WaitForSeconds(delay);
        GenerateQuestion();
    }

    void UpdateScore()
    {
        scoreText.text = $"SCORE: {currentScore}";
    }
}