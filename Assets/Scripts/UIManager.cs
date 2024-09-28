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
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image fillAmount;
    [SerializeField] private RectTransform bucketUI;

    private void Start()
    {
        GameManager.instance.ScoreAction += ScoreAction;
        GameManager.instance.DecreaseHealthAction += DecreaseHealthAction;
        GameManager.instance.SpongeFilled += SpongeFilled;
        GameManager.instance.SpongeFilled += BucketTween;
    }

    private void ScoreAction(int score) => scoreText.text = score.ToString();

    private void DecreaseHealthAction(float value) => fillAmount.fillAmount = value;

    private void SpongeFilled() => fillAmount.fillAmount = 1;

    private void BucketTween()
    {
        bucketUI.localRotation=Quaternion.Euler(0,0,25);
        bucketUI.DOLocalRotate(new Vector3(0, 0, -25), 0.25f)
            .SetLoops(6,LoopType.Yoyo)
            .OnComplete((() => bucketUI.localRotation=quaternion.Euler(0,0,0)));
    }
}
