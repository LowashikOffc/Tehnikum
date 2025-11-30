using UnityEngine;
using UnityEngine.UI; // Обязательно для работы с Image

public class RainbowBackground : MonoBehaviour
{
    [Header("Настройки")]
    [Range(0.01f, 1f)]
    public float speed = 0.1f; // Скорость смены цветов

    [Range(0f, 1f)]
    public float saturation = 0.6f; // Насыщенность (0.5 - пастельные, 1 - яркие кислотные)

    [Range(0f, 1f)]
    public float brightness = 1f; // Яркость

    private Image bgImage;
    private float hue = 0f;

    void Start()
    {
        // Получаем компонент картинки, на которой висит скрипт
        bgImage = GetComponent<Image>();
    }

    void Update()
    {
        // Плавно меняем оттенок (Hue)
        hue += Time.deltaTime * speed;

        // Если оттенок прошел полный круг (больше 1), сбрасываем в 0
        if (hue > 1) hue = 0;

        // Превращаем HSV в обычный RGB цвет и применяем к картинке
        bgImage.color = Color.HSVToRGB(hue, saturation, brightness);
    }
}