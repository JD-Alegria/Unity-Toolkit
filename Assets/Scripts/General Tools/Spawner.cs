using UnityEngine;

public class Spawner : MonoBehaviour
{

    void SpawnObject<TData>(GameObject spawnableObject,
        Vector3 spawnPos,
        Quaternion lookDirection,
        TData data
            ) where TData : ScriptableObject
    {
        GameObject newGO = Instantiate(spawnableObject, spawnPos, lookDirection);
        if (newGO.TryGetComponent(out ISpawnable<TData> spawnable))
            spawnable.Init(data);
    }

    Vector3 GetOffsetPos(Vector3 pos, float offSet)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-offSet, offSet), Random.Range(-offSet, offSet), Random.Range(-offSet, offSet));
        return pos + randomOffset;
    }

    Quaternion GetLookDirection(Vector3 currentposition, Vector3 lookTarget)
    {
        Vector3 direction = lookTarget - currentposition;
        return Quaternion.LookRotation(direction);
    }
}
