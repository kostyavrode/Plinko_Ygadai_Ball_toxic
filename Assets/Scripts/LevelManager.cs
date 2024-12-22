using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
public class LevelManager : MonoBehaviour
{
    public List<GameObject> balls;
    public List<GameObject> cells;
    public LevelData[] levels; // Список всех уровней
    public Transform ballParent; // Родительский объект для шариков
    public Transform cellParent; // Родительский объект для ячеек

    public GameObject boalsBoard;
    public int currentLevelIndex;

    private bool isCanCheckBallsInCells;

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

        LevelData level = levels[levelIndex];

        // Создаем ячейки
        //Debug.Log(level.targetPositions.Length + " targetPositions");
        //for (int i = 0; i < level.targetPositions.Length; i++) 
        //    {
        //    Vector2 pos = level.targetPositions[i];
        //        GameObject cell = Instantiate(level.cellPrefab, pos, Quaternion.identity, cellParent);
        //        cells.Add(cell);
        //    }
        Cell[] tempCells=cellParent.GetComponentsInChildren<Cell>();
        foreach (Cell cell in tempCells)
        {
            cells.Add(cell.gameObject);
        }

            // Создаем шарики
            for (int i = 0; i < level.targetPositions.Length; i++)
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
    private void MoveBallsToStartPosition()
    {
        for (int i=0; i < levels[currentLevelIndex].ballPositions.Length;i++)
        {
            balls[i].GetComponent<Draggable>().MoveTo(levels[currentLevelIndex].ballPositions[i]);
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
            Destroy(child.gameObject);
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
