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
    private AudioSource audioSource;

    private void Awake()
    {
        playerImage = transform.GetChild(1).GetComponent<Image>();
        playerScore = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("soundVolume");
    }

    public bool SetupBoard(Sprite playerImage, int playerScore)
    {
        if (playerScore == 0) Destroy(gameObject);
        if (isAssign)
            return true;

        this.playerImage.sprite = playerImage;
        this.playerScore.text = playerScore.ToString();
        isAssign = true;
        StartCoroutine(PlaySound());
        return false;
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.35f);
        while (transform.localScale.x < 0.5f)
        {
            yield return null;
        }

        audioSource.Play();
    }
}