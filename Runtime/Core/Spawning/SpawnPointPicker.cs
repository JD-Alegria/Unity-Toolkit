using System.Collections.Generic;
using UnityEngine;

namespace Jaleg.Toolkit;

public class SpawnPointPicker : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints = new();
    [SerializeField] Vector3 randomOffsetRange;

    public IReadOnlyList<Transform> SpawnPoints => spawnPoints;

    public bool TryGetRandomSpawnPose(out Vector3 position, out Quaternion rotation)
    {
        position = default;
        rotation = default;

        if (spawnPoints.Count == 0) return false;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];
        position = point.position + GetRandomOffset();
        rotation = point.rotation;
        return true;
    }

    Vector3 GetRandomOffset()
    {
        return new Vector3(
            Random.Range(-randomOffsetRange.x, randomOffsetRange.x),
            Random.Range(-randomOffsetRange.y, randomOffsetRange.y),
            Random.Range(-randomOffsetRange.z, randomOffsetRange.z));
    }
}
