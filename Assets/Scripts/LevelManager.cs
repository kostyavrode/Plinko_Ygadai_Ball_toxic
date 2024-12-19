using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
public class LevelManager : MonoBehaviour
{
    public List<GameObject> balls;
    public List<GameObject> cells;
    public LevelData[] levels; // ������ ���� �������
    public Transform ballParent; // ������������ ������ ��� �������
    public Transform cellParent; // ������������ ������ ��� �����
    private int currentLevelIndex;

    private bool isCanCheckBallsInCells;

    void Start()
    {
        LoadLevel(0);
    }
    void Update()
    {
        if (AreAllBallsInCells() && isCanCheckBallsInCells)
        {
            Debug.Log("��� ������ � ������ �������!");
        }
        
    }

    public void LoadLevel(int levelIndex)
    {
        ClearLevel();
        currentLevelIndex = levelIndex;

        LevelData level = levels[levelIndex];

        // ������� ������
        for (int i = 0; i < level.targetPositions.Length; i++) 
            //foreach (var pos in level.targetPositions)
            {
            Vector2 pos = level.targetPositions[i];
                GameObject cell = Instantiate(level.cellPrefab, pos, Quaternion.identity, cellParent);
                cells.Add(cell);
            }

            // ������� ������
            for (int i = 0; i < level.targetPositions.Length; i++)
            //foreach (var pos in level.ballPositions)
            {
                Vector2 pos = level.targetPositions[i];
                GameObject newball = Instantiate(level.ballPrefab, pos, Quaternion.identity, ballParent);
                balls.Add(newball);
                newball.GetComponent<Draggable>().targetCell = cells[i].GetComponent<Cell>();
            newball.GetComponent<Draggable>().Blink();
            }
            //BlinkBalls();
            Invoke("MoveBallsToStartPosition", 3);
        }
    private void BlinkBalls()
    {
        for (int i = 0; i < levels[currentLevelIndex].ballPositions.Length; i++)
        {
            balls[i].GetComponent<Draggable>().Blink();
        }
    }
    private void MoveBallsToStartPosition()
    {
        for (int i=0; i < levels[currentLevelIndex].ballPositions.Length;i++)
        {
            balls[i].GetComponent<Draggable>().MoveTo(levels[currentLevelIndex].ballPositions[i]);
        }
        StartCoroutine(WaitForGameStart());
    }
    private void ClearLevel()
    {
        foreach (Transform child in ballParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in cellParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void CheckCompletion()
    {
        // ���������, ��� �� ������ ��������� � ���������� �������
        // ����� ����������� ��� �����
    }
    bool AreAllBallsInCells()
    {
        foreach (var ball in balls)
        {
            Draggable draggable = ball.GetComponent<Draggable>();
            if (!draggable.isInCell)
            {
                return false; // ���� ���� �� ���� ����� �� � ������, ���������� false
            }
        }
        return true; // ��� ������ � �������
    }
    private IEnumerator WaitForGameStart()
    {
        yield return new WaitForSeconds(2);
        isCanCheckBallsInCells=true;
    }
}
