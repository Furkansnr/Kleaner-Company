using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private int playersCount = 2;
    [HideInInspector] public string[] playerIDs = { "Player1", "Player2", "Player3", "Player4" };
    public Sprite[] playersSprite;
    public static GameData instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public int GetPlayersCount() => playersCount;

    public void SetPlayerCount(int value) => playersCount = value;
}