using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] private GameObject Map;

    public bool isMapOpen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        CloseMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {

            if (!isMapOpen)
            {
                OpenMap();
            }
            else
            {
                CloseMap();
            }
        }
    }

    private void OpenMap()
    {
        Map.SetActive(true);
        isMapOpen = true;
    }
    private void CloseMap()
    {
        Map.SetActive(false);
        isMapOpen = false;
    }
}
