using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    private float fixedY; // ������������� ������ ������

    void Start()
    {
        offset = transform.position - player.position;
        //fixedY = transform.position.y; // ���������� ��������� ������
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            //targetPosition.y = fixedY; // ��������� Y ����������
            transform.position = targetPosition;
        }
    }
}
