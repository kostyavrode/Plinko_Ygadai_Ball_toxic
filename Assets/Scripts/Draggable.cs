using UnityEngine;
using DG.Tweening;
public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private bool isDragging = false;
    private Rigidbody2D rb;
    private Collider2D collider2D;

    public Sprite[] sprites;

    public float timeForMoving=1.5f;

    public bool isInCell = false;

    public Cell targetCell;

    private Vector2 startPosition;

    private Tween currentTween;

    private bool isCanDrag;

    void Start()
    {
        mainCamera = Camera.main;  // Получаем основную камеру
        rb = GetComponent<Rigidbody2D>();  // Получаем Rigidbody2D (если есть)
        collider2D = GetComponent<Collider2D>();  // Получаем Collider2D
        GetComponent<SpriteRenderer>().sprite= sprites[Random.Range(0,sprites.Length)];
    }
    public void Blink()
    {
        transform.DOScale(1.5f,1.5f).SetEase(Ease.InBounce).SetLoops(2,LoopType.Yoyo);
    }
    public void MoveTo(Vector3 pos)
    {
        transform.DOMove(pos, 1.5f).OnComplete(()=> { startPosition = transform.position; isCanDrag = true; });
    }

    // Для мобильных устройств (касание)
    void Update()
    {
        if (isCanDrag)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {

                    currentTween.Kill();
                    //startPosition= transform.position;
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
                    if (!CheckIfInCell())
                    {
                        currentTween = transform.DOMove(startPosition, 1).SetEase(Ease.InOutQuad).OnComplete(() =>
                        {
                            currentTween = null;
                        });

                    }
                    else
                    {
                        isCanDrag = false;
                        transform.DOMove(targetCell.transform.position, 0.3f);
                    }
                }
            }
        }
    }
    bool CheckIfInCell()
    {
        if (targetCell != null && collider2D.bounds.Intersects(targetCell.GetComponent<Collider2D>().bounds))
        {
            isInCell = true;
            return true;
        }
        else
        {
            isInCell = false;
            return false;
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
