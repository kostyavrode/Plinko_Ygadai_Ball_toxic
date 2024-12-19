using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool IsCorrect(GameObject ball)
    {
        // ���������� ������� ������ � �������� ������
        return Vector2.Distance(transform.position, ball.transform.position) < 0.5f; // ����� ��������
    }
}
