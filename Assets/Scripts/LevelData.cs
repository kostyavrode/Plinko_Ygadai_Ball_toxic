using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public Transform[] ballPositions; // ��������� ������� �������
    public Cell[] targetCell; // ������� ������� �������
    public GameObject ballPrefab; // ������ ������
    public GameObject cellPrefab; // ������ ������
}