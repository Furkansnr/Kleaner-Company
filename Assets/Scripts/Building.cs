using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject dirtPrefab;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject[] rooms;
    private int[] spawnChances={ 50, 50, 50, 50 };


    private void OnEnable()
    {
        GameManager.instance.GameEnd+=GameEnd;
    }
    
    private void OnDisable()
    {
        GameManager.instance.GameEnd-=GameEnd;
    }



    private void Update()
    {
        transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
    }

    public void ResetRoomPosition(Transform room)
    {
        Vector3 newPosition= new Vector3(room.transform.localPosition.x,
            room.transform.localPosition.y + 4.05f);
        Destroy(room.gameObject);
        GameObject newRoom = Instantiate(GetRandomRoom(), transform);
        newRoom.transform.localPosition = newPosition;
        Transform[] spawnPoints = newRoom.GetComponent<Room>().spawnPoints;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            DestroySpawnChilds(spawnPoints, i);
            SpawnDirt(spawnPoints, i);
        }
    }

    private GameObject GetRandomRoom() => rooms[Random.Range(0, rooms.Length - 1)];

    private void DestroySpawnChilds(Transform[] spawnPoints, int i)
    {
        // Destory spawn points childs
        if (spawnPoints[i].childCount > 0)
        {
            for (int j = 0; j < spawnPoints[i].childCount; j++)
            {
                Destroy(spawnPoints[i].GetChild(j).gameObject);
            }
        }
    }
    private void SpawnDirt(Transform[] spawnPoints, int i)
    {
        // Spawn new dirt object or increase spawn chance for later
        if (spawnChances[i] > Random.Range(0, 101))
        {
            GameObject dirt = Instantiate(dirtPrefab, spawnPoints[i]);
            dirt.transform.localPosition = new Vector3(-0.05f,0,0);
            dirt.transform.localRotation=Quaternion.Euler(0,0,90);
        }
        else
        {
            spawnChances[i] += 20;
        }
    }

    private void GameEnd()
    {
        Destroy(this);
    }
}