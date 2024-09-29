using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private Image playerImage;
    private TextMeshProUGUI playerScore;
    private bool isAssign;

    private void Awake()
    {
        playerImage = transform.GetChild(0).GetComponent<Image>();
        playerScore = GetComponentInChildren<TextMeshProUGUI>();
    }

    public bool SetupBoard(Sprite playerImage, int playerScore)
    {
        if (isAssign)
        {
            return true;
        }
        this.playerImage.sprite = playerImage;
        this.playerScore.text = playerScore.ToString();
        isAssign = true;
        return false;
    }
}