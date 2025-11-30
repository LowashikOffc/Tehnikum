using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathGame : MonoBehaviour
{
    [Header("UI Компоненты")]
    public TMP_Text questionText;
    public TMP_Text scoreText;
    public TMP_Text feedbackText;
    public TMP_InputField inputField;

    [Header("Настройки сложности")]
    public int maxNumber = 10;

    private int correctAnswer;
    private int score = 0;

    void Start()
    {
        // --- ЭТИ ДВЕ СТРОЧКИ ВКЛЮЧАЮТ КУРСОР ---
        Cursor.lockState = CursorLockMode.None; // Разблокировать курсор (чтобы двигался)
        Cursor.visible = true; // Сделать видимым
        // ---------------------------------------

        score = 0;
        feedbackText.text = "";
        UpdateScore();
        GenerateQuestion();
    }

    void Update()
    {
        // Добавляем поддержку нажатия Enter (основной и на NumPad)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckAnswer();
        }
    }

    void GenerateQuestion()
    {
        // Исправление: maxNumber + 1, чтобы само число maxNumber тоже могло выпасть
        int a = Random.Range(1, maxNumber + 1);
        int b = Random.Range(1, maxNumber + 1);

        int operation = Random.Range(0, 2);

        if (operation == 0) // Сложение
        {
            correctAnswer = a + b;
            questionText.text = $"{a} + {b} = ?";
        }
        else // Вычитание
        {
            if (a < b) { int temp = a; a = b; b = temp; }

            correctAnswer = a - b;
            questionText.text = $"{a} - {b} = ?";
        }

        // Очищаем поле и ВЕРТАЕМ ФОКУС, чтобы можно было сразу печатать
        inputField.text = "";
        inputField.ActivateInputField();
        inputField.Select();
    }

    public void CheckAnswer()
    {
        // Если пусто, ничего не делаем
        if (string.IsNullOrEmpty(inputField.text)) return;

        int playerAnswer;

        if (!int.TryParse(inputField.text, out playerAnswer))
        {
            feedbackText.color = Color.yellow; // Желтый для предупреждения
            feedbackText.text = "Это не число!";
            inputField.text = "";
            inputField.ActivateInputField(); // Возвращаем фокус
            return;
        }

        if (playerAnswer == correctAnswer)
        {
            score++;
            // Исправление: используем только .color, без html тегов
            feedbackText.color = Color.green;
            feedbackText.text = "Верно!";

            UpdateScore();
            GenerateQuestion();
        }
        else
        {
            feedbackText.color = Color.red;
            feedbackText.text = "Ошибка! Попробуй еще.";

            inputField.text = "";
            inputField.ActivateInputField(); // Возвращаем фокус, чтобы игрок исправил
            inputField.Select();
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Счет: " + score;
    }

    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    // Добавь это в конец скрипта MathGame, перед последней закрывающей скобкой }

    public void BackToMainWorld()
    {
        // ВНИМАНИЕ: Напиши здесь точное имя твоей 3D сцены (Level1, MainScene и т.д.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}