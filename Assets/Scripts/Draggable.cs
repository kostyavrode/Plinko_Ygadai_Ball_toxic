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
        mainCamera = Camera.main;  // Получаем основную камеру
        rb = GetComponent<Rigidbody2D>();  // Получаем Rigidbody2D (если есть)
        collider2D = GetComponent<Collider2D>();  // Получаем Collider2D
    }

    void OnMouseDown()
    {
        
        // Проверка на наличие Collider2D для перетаскивания
        if (collider2D == null)
        {
            Debug.LogWarning("Collider2D не найден. Добавьте Collider2D на объект.");
            return;
        }

        if (isDragging) return;

        isDragging = true;

        // Сохраняем смещение между позицией мыши и объектом
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        // Перемещаем объект в позицию мыши, с учетом смещения
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
        // Получаем позицию мыши на экране и преобразуем её в мировые координаты
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Расстояние от камеры
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    // Для мобильных устройств (касание)
    void Update()
    {
        CheckIfInCell();
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Проверка касания для UI элементов или коллайдеров
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
            Debug.Log($"{gameObject.name} вошел в {targetCell.gameObject.name}");
        }
    }

    // Проверяем, выходит ли шарик из ячейки
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == targetCell.gameObject)
        {
            isInCell = false;
            Debug.Log($"{gameObject.name} покинул {targetCell.gameObject.name}");
        }
    }
}
