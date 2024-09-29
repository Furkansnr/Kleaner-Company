using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaterBucket : MonoBehaviour
{
    [SerializeField] private ParticleSystem waterParticle;
    [SerializeField] private ParticleSystem succesfullParticle;
    private string playerID;

    private void OnEnable()
    {
        GameManager.instance.SkillCheckSuccesfull += DestroyWaterBucket;
        GameManager.instance.GameEnd += DestroyAnimation;
    }

    private void OnDisable()
    {
        GameManager.instance.SkillCheckSuccesfull -= DestroyWaterBucket;
        GameManager.instance.GameEnd -= DestroyAnimation;
    }

    public void SetupWaterBucket(string playerID)
    {
        this.playerID = playerID;
    }


    private void DestroyWaterBucket(string playerID, int value)
    {
        if (this.playerID != playerID) return;
        succesfullParticle.Play();
        if (value == 5)
            DestroyAnimation();
    }

    private void DestroyAnimation()
    {
        waterParticle.Stop();
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad)
            .OnComplete((() => Destroy(gameObject)))
            .SetDelay(1f);
    }
}