using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject mapSprite;

    private bool visited = false;
    //private static GameData gameData; // ����� ������ ��� ���� ������

    private void Awake()
    {
       /* // ��������� ������ ������ ���� ��� (��� ������ �������)
        if (gameData == null)
        {
            gameData = SaveManager.Load();
        }*/
    }

    private void Start()
    {
        mapSprite.SetActive(false);

        /* // ���������, ���� �� ������� �������� �����
         if (gameData.visitedRooms.Contains(gameObject.name))
         {
             visited = true;
             if (mapSprite != null)
                 mapSprite.SetActive(true);
         }
         else
         {
             if (mapSprite != null)
                 mapSprite.SetActive(false);
         }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam?.SetActive(true);
            mapSprite.SetActive(true);

            /*if (!visited)
            {
                visited = true;
                if (mapSprite != null)
                    mapSprite.SetActive(true);

                *//*// ��������� ������� � ������ � ���������
                if (!gameData.visitedRooms.Contains(gameObject.name))
                {
                    gameData.visitedRooms.Add(gameObject.name);
                    SaveManager.Save(gameData);
                }

                Debug.Log($"[Room] {gameObject.name} ��������� � ����������.");*//*
            }*/
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam?.SetActive(false);
        }
    }
}
