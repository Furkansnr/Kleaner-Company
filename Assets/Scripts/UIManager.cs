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
    [SerializeField] private Transform[] skillChecks;
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private Image[] fillAmounts;
    [SerializeField] private Image[] playerSprites;
    [SerializeField] private Board[] boards;
    [SerializeField] private Transform[] buttons;
    [SerializeField] private TextMeshProUGUI gameTimer;
    [SerializeField] private GameObject gameEndPanel;
    private string[] playerIDs;


    private void OnEnable()
    {
        GameManager.instance.ScoreAction += ScoreAction;
        GameManager.instance.DecreaseHealthAction += DecreaseHealthAction;
        GameManager.instance.SpongeFilled += SpongeFilled;
        GameManager.instance.PlayersAction += SetupPanels;
        GameManager.instance.PlayersAction += SetupSkillChecks;
        GameManager.instance.DecreaseTimerAction += DecreaseTimer;
        GameManager.instance.GameEnd += GameEnd;
    }

    private void OnDisable()
    {
        GameManager.instance.ScoreAction -= ScoreAction;
        GameManager.instance.DecreaseHealthAction -= DecreaseHealthAction;
        GameManager.instance.SpongeFilled -= SpongeFilled;
        GameManager.instance.PlayersAction -= SetupPanels;
        GameManager.instance.PlayersAction -= SetupSkillChecks;
        GameManager.instance.DecreaseTimerAction -= DecreaseTimer;
        GameManager.instance.GameEnd -= GameEnd;
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
                panels[0].anchoredPosition = new Vector2(-635, -470);
                panels[1].anchoredPosition = new Vector2(-190, -470);
                panels[2].anchoredPosition = new Vector2(250, -470);
                panels[3].anchoredPosition = new Vector2(690, -470);
                break;
        }
    }
    
    private void SetupSkillChecks(int value)
    {
        switch (value)
        {
            case 1:
                skillChecks[0].transform.localPosition = new Vector2(-140, -488);
                break;
            case 2:
                skillChecks[0].transform.localPosition = new Vector2(-400, -488);
                skillChecks[1].transform.localPosition = new Vector2(360, -488);
                break;
            case 3:
                skillChecks[0].transform.localPosition = new Vector2(-450, -488);
                skillChecks[1].transform.localPosition = new Vector2(115, -488);
                skillChecks[2].transform.localPosition = new Vector2(615, -488);
                break;
            case 4:
                skillChecks[0].transform.localPosition = new Vector2(-490, -200);
                skillChecks[1].transform.localPosition = new Vector2(-40, -200);
                skillChecks[2].transform.localPosition = new Vector2(405, -200);
                skillChecks[3].transform.localPosition = new Vector2(850, -200);
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

    private void DecreaseTimer(float timer)
    {
        gameTimer.text = timer.ToString("F0");
    }

    private void GameEnd()
    {
        gameEndPanel.SetActive(true);
        int[] playerScores = GameManager.instance.GetPlayerScores();
        Sprite[] playersSprite = GameData.instance.playersSprite;
        int[] playersNumber = { 0, 0, 0, 0 };
        Array.Copy(playerScores, playersNumber, playerScores.Length);
        Array.Sort(playerScores);
        Array.Reverse(playerScores);

        for (int i = 0; i < boards.Length; i++)
        {
            for (int j = 0; j < boards.Length; j++)
            {
                if (playersNumber[i] == playerScores[j])
                {
                    bool canBreak = boards[j].SetupBoard(playersSprite[i], playerScores[j]);
                    if (!canBreak) break;
                }
            }
        }
        RankingAnimation();
    }
    
    private void RankingAnimation()
    {
        RectTransform[] boardsRect=new RectTransform[4];
        for (int i = 0; i < boards.Length; i++)
        {
            boardsRect[i] = boards[i].GetComponent<RectTransform>();
        }

        Sequence q = DOTween.Sequence();
        q.Append(gameEndPanel.transform.DOScale(Vector3.zero, 0.75f).From().SetEase(Ease.OutCubic));
        for (int i = 0; i < boardsRect.Length; i++)
        {
            q.Append(boardsRect[i].transform.DOScale(Vector3.zero, 0.75f).From()
                .SetEase(Ease.OutBack));
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            q.Append(buttons[i].DOScale(Vector3.zero, 0.75f).From()
                .SetEase(Ease.OutCubic));
        }

        q.Play();
    }
}