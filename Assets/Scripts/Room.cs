using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] spawnPoints;
    private Building building;

    private void Awake()
    {
        building = transform.parent.GetComponent<Building>();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            building.ResetRoomPosition(transform);
        }
    }
}