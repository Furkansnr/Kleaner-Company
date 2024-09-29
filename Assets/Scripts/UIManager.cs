using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform[] panels;
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private Image[] fillAmounts;
    [SerializeField] private Image[] playerSprites;
    private string[] playerIDs;


    private void OnEnable()
    {
        GameManager.instance.ScoreAction += ScoreAction;
        GameManager.instance.DecreaseHealthAction += DecreaseHealthAction;
        GameManager.instance.SpongeFilled += SpongeFilled;
        GameManager.instance.PlayersAction += SetupPanels;
    }

    private void OnDisable()
    {
        GameManager.instance.ScoreAction -= ScoreAction;
        GameManager.instance.DecreaseHealthAction -= DecreaseHealthAction;
        GameManager.instance.SpongeFilled -= SpongeFilled;
        GameManager.instance.PlayersAction -= SetupPanels;
    }

    private void Start()
    {
        playerIDs = GameData.instance.playerIDs;
        SetPlayerSprite();
    }

    private void ScoreAction(string playerID, int score)
    {
        int index = 0;
        for (int i = 0; i < playerIDs.Length; i++)
        {
            if (playerIDs[i] == playerID) index = i;
        }

        scoreTexts[index].text = score.ToString();
    }

    private void DecreaseHealthAction(string playerID, float value)
    {
        for (int i = 0; i < playerIDs.Length; i++)
        {
            if (playerID == playerIDs[i])
            {
                fillAmounts[i].fillAmount = value;
                break;
            }
        }
    }

    private void SpongeFilled(string playerID)
    {
        for (int i = 0; i < playerIDs.Length; i++)
        {
            if (playerID == playerIDs[i])
            {
                fillAmounts[i].fillAmount = 1;
                break;
            }
        }
    }

    private void SetupPanels(int value)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < value; i++)
        {
            panels[i].gameObject.SetActive(true);
        }

        switch (value)
        {
            case 1:
                panels[0].anchoredPosition = new Vector2(-140, -488);
                break;
            case 2:
                panels[0].anchoredPosition = new Vector2(-400, -488);
                panels[1].anchoredPosition = new Vector2(360, -488);
                break;
            case 3:
                panels[0].anchoredPosition = new Vector2(-450, -488);
                panels[1].anchoredPosition = new Vector2(115, -488);
                panels[2].anchoredPosition = new Vector2(615, -488);
                break;
            case 4:
                panels[0].anchoredPosition = new Vector2(-668, -488);
                panels[1].anchoredPosition = new Vector2(-238, -488);
                panels[2].anchoredPosition = new Vector2(192, -488);
                panels[3].anchoredPosition = new Vector2(622, -488);
                break;
        }
    }

    private void SetPlayerSprite()
    {
        for (int i = 0; i < playerSprites.Length; i++)
        {
            playerSprites[i].sprite = GameData.instance.playersSprite[i];
        }
    }
}