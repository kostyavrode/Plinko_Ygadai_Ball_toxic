using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;
public class LevelManager : MonoBehaviour
{
    public List<GameObject> balls;
    public List<Cell> cells;
    public LevelData[] levels; // ������ ���� �������
    public Transform ballParent; // ������������ ������ ��� �������
    public Transform cellParent; // ������������ ������ ��� �����

    public int time;
    public GameObject boalsBoard;
    public Transform[] ballsBoardPositions;
    public int currentLevelIndex;

    private Timer timer;
    public float normalizedTime;
    private bool isCanCheckBallsInCells;

    private void Awake()
    {
        if (cells.Count <= 0)
        {
            cells = cellParent.GetComponentsInChildren<Cell>().ToList<Cell>();
        }
        foreach (Cell cell in cells)
        {
            //cells.Add(cell);
            cell.gameObject.SetActive(false);
        }
        timer = new Timer();
    }

    void Start()
    {

        //LoadLevel(0);
    }
    void Update()
    {
        if (AreAllBallsInCells() && isCanCheckBallsInCells)
        {
            
            UITemplate.instance.EndGame(true);
            isCanCheckBallsInCells = false;
        }
        if (isCanCheckBallsInCells && GameManager.instance.gameState==GameState.PLAYING)
        {
            timer.UpdateTimer();
            normalizedTime=timer.GetNormalizedTime();
            UITemplate.instance.ShowTimer(normalizedTime);
        }
        
    }
    public void SetCurrentLevel(int level)
    {
        currentLevelIndex = level;
    }
    public void LoadLevel(int levelIndex)
    {
        ClearLevel();
        currentLevelIndex = levelIndex;
        boalsBoard.SetActive(true);
        timer.ResetTimer();
        timer.StartTimer();
        LevelData level = levels[levelIndex];

        // ������� ������
        //Debug.Log(level.targetPositions.Length + " targetPositions");
        //for (int i = 0; i < level.targetPositions.Length; i++) 
        //    {
        //    Vector2 pos = level.targetPositions[i];
        //        GameObject cell = Instantiate(level.cellPrefab, pos, Quaternion.identity, cellParent);
        //        cells.Add(cell);
        //    }
       
        foreach (Cell cell in cells)
        {
            //cells.Add(cell);
            cell.gameObject.SetActive(true);
        }

            // ������� ������
            for (int i = 0; i < level.targetCell.Length; i++)
            {
                Vector2 pos = level.targetCell[i].transform.position;
                GameObject newball = Instantiate(level.ballPrefab, pos, Quaternion.identity, ballParent);
                balls.Add(newball);
                newball.GetComponent<Draggable>().targetCell = level.targetCell[i];
            newball.GetComponent<Draggable>().Blink();
            }
            //BlinkBalls();
            Invoke("MoveBallsToStartPosition", 3);
        }
    private void MoveBallsToStartPosition()
    {
        for (int i=0; i < levels[currentLevelIndex].ballPositions.Length;i++)
        {
            balls[i].GetComponent<Draggable>().MoveTo(levels[currentLevelIndex].ballPositions[i].position);
        }
        StartCoroutine(WaitForGameStart());
    }
    public void ClearLevel()
    {
        boalsBoard.SetActive(false);
        foreach (Transform child in ballParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in cellParent)
        {

            child.gameObject.SetActive(false);
            //cells.Clear();
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

public class Timer
{
    public float timeRemaining = 30f; // ����� ������� � ��������
    public float totalTime = 30f; // ����� ����� �������
    public bool timerIsRunning = false; // ����, �����������, �������� �� ������

    public void UpdateTimer()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // ��������� ���������� �����
            }
            else
            {
                timeRemaining = 0; // ������������� 0 ��� ��������
                timerIsRunning = false; // ������������� ������
                TimerEnded();
            }
        }
    }

    // ������ �������
    public void StartTimer()
    {
        timerIsRunning = true;
    }

    // ����� �������
    public void ResetTimer(float newTime = 30f)
    {
        totalTime = newTime; // ������������� ����� ����� �����
        timeRemaining = newTime;
        timerIsRunning = false;
    }

    // ���������� �������� ������� � ��������� �� 1 �� 0
    public float GetNormalizedTime()
    {
        return Mathf.Clamp01(timeRemaining / totalTime); // ����������� ��������
    }

    // �������� �� ���������� �������
    private void TimerEnded()
    {
        Debug.Log("����� �������!");
        if (GameManager.instance.gameState == GameState.PLAYING)
        {
            UITemplate.instance.EndGame(false);
        }
    }
}
