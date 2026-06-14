using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


// can spawn either C# runtime objects or gameObjects
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    
    // scriptableObjects Here
    [Header("Data References")]

    // gameObject prefabs here
    [Header("Spawnable Prefabs")]
    [Space]
    //any position references here (like spawnpoints)
    [Header("Position References")]
    Transform spawnPos;
    
    public event Action OnObjectSpawned;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    /* spawning C# runtime object only
    public void CreateRuntimeObject()
    {
        ShipRequest request = ShipRequestFactory.CreateRandom(fighterData);
        
        OnShipRequestCreated?.Invoke(request);
    }*/

    //spawn gameObject
    
    void SpawnObject(GameObject spawnableObject)
    {
        //get spawn position
        Vector3 spawnPos = Vector3.zero;
        
        // instantiate Object
        GameObject newGO = Instantiate(spawnableObject, spawnPos, Quaternion.identity);
        
        // set up object via controller init
        
        //fire event that object was spawned, pass in controller script
        OnObjectSpawned?.Invoke();
    }
}
