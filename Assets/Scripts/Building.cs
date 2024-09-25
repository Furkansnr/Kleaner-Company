using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject dirtPrefab;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform[] spawnPoints;
    private float _deadZoneY = -6.14f;
    private Vector3 _firstPos;

    private void Start()
    {
        _firstPos = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition += new Vector3(0, -speed * Time.deltaTime, 0);
        
        if(transform.localPosition.y < _deadZoneY) ResetPosition();
    }

    private void ResetPosition()
    {
        transform.localPosition = _firstPos;

        int spawnChance = 50;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if(spawnPoints[i].childCount > 0) Destroy(spawnPoints[i].GetChild(0).gameObject);
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnChance > Random.Range(0, 101))
            {
                spawnChance = 50;
               GameObject dirt= Instantiate(dirtPrefab, spawnPoints[i]);
               dirt.transform.localPosition=Vector3.zero;
               dirt.transform.rotation=Quaternion.Euler(0,0,-90);
            }
            else
            {
                spawnChance += 20;
            }
        }
    }
}