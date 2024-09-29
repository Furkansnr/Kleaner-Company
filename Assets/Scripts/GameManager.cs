using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int[] playerScores={0,0,0,0};
    private string[] playerIDs;
    public Action<string, int> ScoreAction;
    public Action<string, float> DecreaseHealthAction;
    public Action<string> SpongeFilled;
    public Action <string> SkillCheckAppears;
    public Action<string, int> SkillCheckSuccesfull;
    public Action<int> PlayersAction;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        EmitPlayersAction(GameData.instance.GetPlayersCount());
        playerIDs = GameData.instance.playerIDs;
    }

    public void CalculateScore(string playerID, int value)
    {
        if (playerID == "") return;
        int tScore = 0;
        switch (value)
        {
            case var n when (n < 25):
                tScore = 10;
                break;
            case var n when (n < 50):
                tScore = 8;
                break;
            case var n when (n < 150):
                tScore = 5;
                break;
            case var n when (n < 300):
                tScore = 2;
                break;
            default:
                tScore = 1;
                break;
        }

        EmitScoreAction(playerID, tScore);
    }

    public void EmitDecreaseHealthAction(string playerID ,float value) => DecreaseHealthAction?.Invoke(playerID ,value);

    public void EmitSpongeFilled(string playerID) => SpongeFilled?.Invoke(playerID);

    public void OpenSkillCheckPanelManager(string playerID) => SkillCheckAppears?.Invoke(playerID);

    public void EmitSkillCheckSuccessfull(string playerID, int value) => SkillCheckSuccesfull?.Invoke(playerID, value);

    void EmitScoreAction(string playerID, int score)
    {
        int totalscore = 0;
        for (int i = 0; i < playerIDs.Length; i++)
        {
            if (playerIDs[i] == playerID)
            {
                totalscore = playerScores[i];
                playerScores[i] += score;
                break;
            }
        }
        totalscore += score;
        ScoreAction?.Invoke(playerID, totalscore);
    }

    private void EmitPlayersAction(int value) => PlayersAction?.Invoke(value);
}