using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapRoomManager : MonoBehaviour
{
    public static MapRoomManager instance;

    [System.Serializable]
    public class RoomData
    {
        public string roomName;
        public GameObject roomObject;
        public bool isVisited;
    }

    public List<RoomData> rooms = new List<RoomData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Инициализируем все комнаты как скрытые
        foreach (var room in rooms)
        {
            if (room.roomObject != null && !room.isVisited)
            {
                room.roomObject.SetActive(false);
            }
        }
    }

    // Открыть комнату по имени
    public void RevealRoom(string roomName)
    {
        RoomData room = rooms.Find(r => r.roomName == roomName);
        if (room != null && !room.isVisited)
        {
            room.isVisited = true;
            if (room.roomObject != null)
            {
                room.roomObject.SetActive(true);
            }
        }
    }

    // Открыть комнату при входе в триггер
    public void RevealRoom(GameObject roomObject)
    {
        RoomData room = rooms.Find(r => r.roomObject == roomObject);
        if (room != null && !room.isVisited)
        {
            room.isVisited = true;
            room.roomObject.SetActive(true);
        }
    }
}
