using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public Transform[] ballPositions; // Начальные позиции шариков
    public Cell[] targetCell; // Целевые позиции шариков
    public GameObject ballPrefab; // Префаб шарика
    public GameObject cellPrefab; // Префаб ячейки
}