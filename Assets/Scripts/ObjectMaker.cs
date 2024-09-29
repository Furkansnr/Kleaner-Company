using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObjectMaker : MonoBehaviour
{
    [SerializeField] private GameObject waterBucketPrefab;
    [SerializeField] private float upperWaterBucketPos = 0.1f;
    [SerializeField] private float upperSpongePos = 0.5f;
    private float waterBucketYAxis;
    private GameObject waterBucket;
    public static ObjectMaker instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }


    public Vector3 SpawnWaterBucket(Vector3 spongePos, string playerID)
    {
        waterBucket = Instantiate(waterBucketPrefab);
        waterBucket.transform.position = new Vector3(spongePos.x,
            spongePos.y + upperWaterBucketPos,
            spongePos.z);
        Vector3 newPos = new Vector3(waterBucket.transform.position.x, spongePos.y + upperSpongePos,
            waterBucket.transform.position.z);
        waterBucket.transform.DOScale(Vector3.zero, 0.5f).
            SetEase(Ease.InOutQuad)
            .From();
        waterBucketYAxis = waterBucket.transform.position.y;
        waterBucket.GetComponent<WaterBucket>().SetupWaterBucket(playerID);
        return newPos;
    }

    public float GetWaterBucketYAxis() => waterBucketYAxis;
}