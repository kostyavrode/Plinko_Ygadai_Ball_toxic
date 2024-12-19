using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public Vector2[] ballPositions; // Начальные позиции шариков
    public Vector2[] targetPositions; // Целевые позиции шариков
    public GameObject ballPrefab; // Префаб шарика
    public GameObject cellPrefab; // Префаб ячейки
}