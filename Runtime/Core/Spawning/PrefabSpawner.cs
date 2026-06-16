using System;
using UnityEngine;

namespace Jaleg.Toolkit;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] SpawnPointPicker spawnPointPicker;

    public event Action<GameObject> Spawned;

    public GameObject Spawn()
    {
        if (prefab == null) return null;

        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        if (spawnPointPicker != null && spawnPointPicker.TryGetRandomSpawnPose(out Vector3 spawnPosition, out Quaternion spawnRotation))
        {
            position = spawnPosition;
            rotation = spawnRotation;
        }

        GameObject instance = Instantiate(prefab, position, rotation);
        Spawned?.Invoke(instance);
        return instance;
    }
}
