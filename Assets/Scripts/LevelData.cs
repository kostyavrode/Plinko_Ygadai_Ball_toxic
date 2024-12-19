using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public Vector2[] ballPositions; // ��������� ������� �������
    public Vector2[] targetPositions; // ������� ������� �������
    public GameObject ballPrefab; // ������ ������
    public GameObject cellPrefab; // ������ ������
}