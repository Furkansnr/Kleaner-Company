using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialFloors;
    [SerializeField] private Transform buildTransform;
    private Queue<GameObject> floorList=new Queue<GameObject>();
    private int floorCount;
    
    public GameObject[] floorPrefabs;
    
    void Awake()
    {
        for (int i = 0; i < tutorialFloors.Length; i++)
        {
            floorList.Enqueue(tutorialFloors[i]);
            floorCount++;
            print(floorCount);
        }
    }
    
    void Update()
    {
        if (floorList.Count>0)
        {
            GameObject downstairs = floorList.Peek();
            if (downstairs.transform.position.y <= -2.5)
            {
                floorList.Dequeue();
                LeanPool.Despawn(downstairs);
                NewFloor();
            }
        }
    }

    void NewFloor()
    {
        LeanPool.Spawn(floorPrefabs[Random.Range(0, floorPrefabs.Length)],buildTransform); 
        // abi yanlış yerde spawn oluyo level managerin altındaki point noktasında spawn olmalı ama aşağıya kayması için building ing childı olması lazım
        floorCount++;
        print(floorCount);
        // daha sonrasında ilerleyişe göre farklı katların spawnlanması eklenebilir
    }
}
