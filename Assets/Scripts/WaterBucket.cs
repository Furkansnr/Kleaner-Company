using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaterBucket : MonoBehaviour
{
    [SerializeField] private ParticleSystem waterParticle;
    [SerializeField] private ParticleSystem succesfullParticle;

    private void OnEnable()
    {
        GameManager.instance.SkillCheckSuccesfull += DestroyWaterBucket;
    }

    private void OnDisable()
    {
        GameManager.instance.SkillCheckSuccesfull -= DestroyWaterBucket;

    }


    private void DestroyWaterBucket(int value)
    {
        succesfullParticle.Play();
        if (value == 5)
        {
            waterParticle.Stop();
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad)
                .OnComplete((() => Destroy(gameObject)))
                .SetDelay(1f);
        }
            
    }
}