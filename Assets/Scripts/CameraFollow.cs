using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  
    public Vector3 offset; 

    void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}