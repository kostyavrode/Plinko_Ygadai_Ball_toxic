using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;
public class LevelManager : MonoBehaviour
{
    public List<GameObject> balls;
    public List<Cell> cells;
    public LevelData[] levels; // Список всех уровней
    public Transform ballParent; // Родительский объект для шариков
    public Transform cellParent; // Родительский объект для ячеек

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

        // Создаем ячейки
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

            // Создаем шарики
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
        // Проверить, все ли шарики находятся в правильных ячейках
        // Можно реализовать это позже
    }
    bool AreAllBallsInCells()
    {
        foreach (var ball in balls)
        {
            Draggable draggable = ball.GetComponent<Draggable>();
            if (!draggable.isInCell)
            {
                return false; // Если хотя бы один шарик не в ячейке, возвращаем false
            }
        }
        return true; // Все шарики в ячейках
    }
    private IEnumerator WaitForGameStart()
    {
        yield return new WaitForSeconds(2);
        isCanCheckBallsInCells=true;
    }
}

public class Timer
{
    public float timeRemaining = 30f; // Время таймера в секундах
    public float totalTime = 30f; // Общее время таймера
    public bool timerIsRunning = false; // Флаг, указывающий, работает ли таймер

    public void UpdateTimer()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Уменьшаем оставшееся время
            }
            else
            {
                timeRemaining = 0; // Устанавливаем 0 для точности
                timerIsRunning = false; // Останавливаем таймер
                TimerEnded();
            }
        }
    }

    // Запуск таймера
    public void StartTimer()
    {
        timerIsRunning = true;
    }

    // Сброс таймера
    public void ResetTimer(float newTime = 30f)
    {
        totalTime = newTime; // Устанавливаем новое общее время
        timeRemaining = newTime;
        timerIsRunning = false;
    }

    // Возвращает значение времени в диапазоне от 1 до 0
    public float GetNormalizedTime()
    {
        return Mathf.Clamp01(timeRemaining / totalTime); // Нормализуем значение
    }

    // Действие по завершении таймера
    private void TimerEnded()
    {
        Debug.Log("Время истекло!");
        if (GameManager.instance.gameState == GameState.PLAYING)
        {
            UITemplate.instance.EndGame(false);
        }
    }
}
