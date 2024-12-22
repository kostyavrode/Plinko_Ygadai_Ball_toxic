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

    public GameObject boalsBoard;
    public Transform[] ballsBoardPositions;
    public int currentLevelIndex;

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
                newball.GetComponent<Draggable>().targetCell = cells[i];
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
