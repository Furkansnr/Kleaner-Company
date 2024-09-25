using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image fillAmount;

    private void Start()
    {
        GameManager.instance.ScoreAction += ScoreAction;
        GameManager.instance.DecreaseHealthAction += DecreaseHealthAction;
        GameManager.instance.SpongeFilled += SpongeFilled;
    }

    private void ScoreAction(int score) => scoreText.text = score.ToString();

    private void DecreaseHealthAction(float value) => fillAmount.fillAmount = value;

    private void SpongeFilled() => fillAmount.fillAmount = 1;
}
