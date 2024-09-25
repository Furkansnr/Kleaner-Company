using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject dirtPrefab;
    [SerializeField] private float speed=5f;

    private void Update()
    {
        transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
    }
}