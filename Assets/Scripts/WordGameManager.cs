// WordGameManager.cs - Обновленный скрипт с WordDataEg, счетом и перемешиванием

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WordGameManager : MonoBehaviour
{
    // Ссылки на UI элементы (перетащи из Инспекторе)
    public GameObject gameCanvas;
    public Text wordText;
    public List<Button> answerButtons;
    public Text scoreText; // Поле для отображения счета

    // ИЗМЕНЕНИЕ: Тип списка слов изменен на WordDataEg
    private List<WordDataEg> wordList;
    private int currentWordIndex = 0;

    // Переменная для счета
    private int score = 0;

    // Скрипт движения игрока (Movement - ваше название)
    public Movement playerMovementScript;

    void Start()
    {
        // Получаем список WordDataEg из контейнера
        WordListContainerEg container = GetComponent<WordListContainerEg>();
        if (container != null)
        {
            wordList = container.wordList;
        }

        // Подписываем кнопки на метод обработки нажатия
        for (int i = 0; i < answerButtons.Count; i++)
        {
            int buttonIndex = i;
            answerButtons[i].onClick.AddListener(() => CheckAnswer(buttonIndex));
        }

        gameCanvas.SetActive(false);
        UpdateScoreDisplay();
    }

    void Update()
    {
        // Выход из игры по Escape
        if (gameCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        gameCanvas.SetActive(true);

        // Настройка счета и курсора
        score = 0;
        UpdateScoreDisplay();
        Cursor.lockState = CursorLockMode.None; // Разблокировка мышки
        Cursor.visible = true; // Мышка видна

        // Блокируем движение
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        ShuffleWords(); // Перемешиваем порядок вопросов
        currentWordIndex = 0;
        LoadNewWord();
    }

    public void EndGame()
    {
        gameCanvas.SetActive(false);

        // Возврат курсора в режим игры (3D/FPS)
        Cursor.lockState = CursorLockMode.Locked; // Блокировка мышки
        Cursor.visible = false; // Мышка скрыта

        // Разблокируем движение
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
    }

    // МЕТОД: Обновление текста счета
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Счет: {score}";
        }
    }

    public void CheckAnswer(int buttonIndex)
    {
        if (currentWordIndex >= wordList.Count) return;

        // Получаем выбранный ответ с кнопки
        string selectedAnswer = answerButtons[buttonIndex].GetComponentInChildren<Text>().text;

        // Получаем правильный ответ из WordDataEg
        string correctAnswer = wordList[currentWordIndex].russianTranslation;

        if (selectedAnswer == correctAnswer)
        {
            // Правильный ответ: увеличиваем счет
            score++;
            UpdateScoreDisplay();

            currentWordIndex++;
            LoadNewWord();
        }
        else
        {
            Debug.Log("Неправильно. Попробуй еще!");
        }
    }

    // ВСПОМОГАТЕЛЬНАЯ ФУНКЦИЯ: Перемешивание любого списка
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void ShuffleWords()
    {
        // Перемешивание всего списка вопросов
        ShuffleList(wordList);
    }

    private void LoadNewWord()
    {
        if (currentWordIndex < wordList.Count)
        {
            // Текущее слово (тип WordDataEg)
            WordDataEg currentWord = wordList[currentWordIndex];

            wordText.text = currentWord.englishWord;

            // Копируем и перемешиваем опции для кнопок
            List<string> shuffledOptions = new List<string>(currentWord.answerOptions);
            ShuffleList(shuffledOptions);

            for (int i = 0; i < answerButtons.Count; i++)
            {
                if (i < shuffledOptions.Count)
                {
                    // Присваиваем кнопке перемешанный ответ
                    answerButtons[i].GetComponentInChildren<Text>().text = shuffledOptions[i];
                    answerButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    // Скрываем лишние кнопки, если опций не хватает
                    answerButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("Мини-игра завершена! Все слова переведены.");
            EndGame();
        }
    }
}