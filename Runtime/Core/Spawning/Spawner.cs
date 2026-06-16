using UnityEngine;

namespace Jaleg.Toolkit;

public class Spawner : MonoBehaviour
{
    public GameObject SpawnObject<TData>(
        GameObject spawnableObject,
        Vector3 spawnPos,
        Quaternion lookDirection,
        TData data) where TData : ScriptableObject
    {
        GameObject newGO = Instantiate(spawnableObject, spawnPos, lookDirection);
        if (newGO.TryGetComponent(out ISpawnable<TData> spawnable))
        {
            spawnable.Init(data);
        }

        return newGO;
    }

    public GameObject SpawnObject(GameObject spawnableObject, Vector3 spawnPos, Quaternion rotation)
    {
        return Instantiate(spawnableObject, spawnPos, rotation);
    }

    public static Vector3 GetOffsetPos(Vector3 pos, float offset)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-offset, offset),
            Random.Range(-offset, offset),
            Random.Range(-offset, offset));

        return pos + randomOffset;
    }

    public static Quaternion GetLookDirection(Vector3 currentPosition, Vector3 lookTarget)
    {
        Vector3 direction = lookTarget - currentPosition;
        if (direction.sqrMagnitude <= Mathf.Epsilon) return Quaternion.identity;

        return Quaternion.LookRotation(direction);
    }
}
