using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGameScene : MonoBehaviour
{
    [SerializeField] private GameObject[] cameras;
    [SerializeField] private GameObject[] players;
    [SerializeField] private GameObject[] skillChecks;


    private void OnEnable()
    {
        GameManager.instance.PlayersAction += SetupCameras;
        GameManager.instance.PlayersAction += SetupPlayers;
        GameManager.instance.PlayersAction += SetupSkillChecks;
    }

    private void OnDisable()
    {
        GameManager.instance.PlayersAction -= SetupCameras;
        GameManager.instance.PlayersAction -= SetupPlayers;
        GameManager.instance.PlayersAction -= SetupSkillChecks;
    }

    private void SetupCameras(int value)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }
        
        cameras[value-1].SetActive(true);
    }

    private void SetupPlayers(int value)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
        }
        for (int i = 0; i < value; i++)
        {
            players[i].SetActive(true);
        }
    }

    private void SetupSkillChecks(int value)
    {
        string[] playerIDs = GameData.instance.playerIDs;

        for (int i = 0; i < value; i++)
        {
            skillChecks[i].GetComponentInChildren<SkillCheck>().SetPlayerID(playerIDs[i]);
        }
        for (int i = value; i < skillChecks.Length; i++)
        {
            Destroy(skillChecks[i].gameObject);
        }
    }
}
