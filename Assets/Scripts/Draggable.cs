using UnityEngine;
using DG.Tweening;
public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private bool isDragging = false;
    private Rigidbody2D rb;
    private Collider2D collider2D;

    public float timeForMoving=1.5f;

    public bool isInCell = false;

    public Cell targetCell;

    void Start()
    {
        mainCamera = Camera.main;  // �������� �������� ������
        rb = GetComponent<Rigidbody2D>();  // �������� Rigidbody2D (���� ����)
        collider2D = GetComponent<Collider2D>();  // �������� Collider2D
    }

    void OnMouseDown()
    {
        
        // �������� �� ������� Collider2D ��� ��������������
        if (collider2D == null)
        {
            Debug.LogWarning("Collider2D �� ������. �������� Collider2D �� ������.");
            return;
        }

        if (isDragging) return;

        isDragging = true;

        // ��������� �������� ����� �������� ���� � ��������
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        // ���������� ������ � ������� ����, � ������ ��������
        transform.position = GetMouseWorldPosition() + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckIfInCell();
    }
    public void Blink()
    {
        transform.DOScale(1.5f,1.5f).SetEase(Ease.InBounce).SetLoops(2,LoopType.Yoyo);
    }
    public void MoveTo(Vector3 pos)
    {
        transform.DOMove(pos, 1.5f);
    }
    private Vector3 GetMouseWorldPosition()
    {
        // �������� ������� ���� �� ������ � ����������� � � ������� ����������
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // ���������� �� ������
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    // ��� ��������� ��������� (�������)
    void Update()
    {
        CheckIfInCell();
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // �������� ������� ��� UI ��������� ��� �����������
                if (collider2D != null && collider2D.OverlapPoint(mainCamera.ScreenToWorldPoint(touch.position)))
                {
                    offset = transform.position - mainCamera.ScreenToWorldPoint(touch.position);
                    isDragging = true;
                }
            }

            if (touch.phase == TouchPhase.Moved && isDragging)
            {
                transform.position = mainCamera.ScreenToWorldPoint(touch.position) + offset;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }
    void CheckIfInCell()
    {
        if (targetCell != null && collider2D.bounds.Intersects(targetCell.GetComponent<Collider2D>().bounds))
        {
            isInCell = true;

        }
        else
        {
            isInCell = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered"+other.gameObject.name);
        if (other.gameObject == targetCell.gameObject)
        {
            isInCell = true;
            Debug.Log($"{gameObject.name} ����� � {targetCell.gameObject.name}");
        }
    }

    // ���������, ������� �� ����� �� ������
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == targetCell.gameObject)
        {
            isInCell = false;
            Debug.Log($"{gameObject.name} ������� {targetCell.gameObject.name}");
        }
    }
}
