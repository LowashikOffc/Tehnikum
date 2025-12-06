using UnityEngine;

public static class GlobalData
{
    public static bool isReturning = false; // Возвращаемся ли мы из мини-игры?
    public static Vector3 playerPosition;   // Куда возвращать игрока
    public static Quaternion playerRotation; // (Опционально) Куда смотрел игрок
}