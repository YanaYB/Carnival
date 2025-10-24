using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContainerData : MonoBehaviour
{
    public string roomName; // Имя комнаты для открытия на мини-карте

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MapRoomManager.instance.RevealRoom(roomName);
        }
    }
}
