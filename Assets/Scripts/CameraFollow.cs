using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    private float fixedY; // фиксированная высота камеры

    void Start()
    {
        offset = transform.position - player.position;
        //fixedY = transform.position.y; // запоминаем начальную высоту
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            //targetPosition.y = fixedY; // оставляем Y неизменным
            transform.position = targetPosition;
        }
    }
}
