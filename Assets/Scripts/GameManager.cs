using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int _totalScore;
    public Action<int> ScoreAction;
    public Action<float> DecreaseHealthAction;
    public Action SpongeFilled;
    public Action SkillCheckAppears;
    public Action<int> SkillCheckSuccesfull;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void CalculateScore(int value)
    {
        int tScore = 0;
        switch (value)
        {
            case var n when(n < 25):
                tScore = 10;
                break;
            case var n when(n < 50):
                tScore = 8;
                break;
            case var n when(n < 150):
                tScore = 5;
                break;
            case var n when(n < 300):
                tScore = 2;
                break;
            default:
                tScore = 1;
                break;
        }
        EmitScoreAction(tScore);
    }

    public void EmitDecreaseHealthAction(float value) => DecreaseHealthAction?.Invoke(value);

    public void EmitSpongeFilled() => SpongeFilled?.Invoke();

    public void OpenSkillCheckPanelManager() => SkillCheckAppears?.Invoke();

    public void EmitSkillCheckSuccessfull(int value) => SkillCheckSuccesfull?.Invoke(value);
    
    void EmitScoreAction(int score)
    {
        _totalScore += score;
        ScoreAction?.Invoke(_totalScore);
    }
}