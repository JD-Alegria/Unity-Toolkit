using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jaleg.Toolkit;

[Serializable]
public struct WeightedGameObject
{
    public GameObject value;
    [Min(0f)] public float weight;
}

public static class WeightedRandom
{
    public static bool TryPick<T>(IReadOnlyList<T> values, IReadOnlyList<float> weights, out T value)
    {
        value = default;

        if (values == null || weights == null || values.Count == 0 || values.Count != weights.Count)
        {
            return false;
        }

        float totalWeight = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            totalWeight += Mathf.Max(0f, weights[i]);
        }

        if (totalWeight <= 0f) return false;

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < values.Count; i++)
        {
            cumulative += Mathf.Max(0f, weights[i]);

            if (roll <= cumulative)
            {
                value = values[i];
                return true;
            }
        }

        value = values[values.Count - 1];
        return true;
    }

    public static bool TryPickGameObject(IReadOnlyList<WeightedGameObject> entries, out GameObject value)
    {
        value = null;

        if (entries == null || entries.Count == 0) return false;

        float totalWeight = 0f;
        for (int i = 0; i < entries.Count; i++)
        {
            totalWeight += Mathf.Max(0f, entries[i].weight);
        }

        if (totalWeight <= 0f) return false;

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < entries.Count; i++)
        {
            cumulative += Mathf.Max(0f, entries[i].weight);

            if (roll <= cumulative)
            {
                value = entries[i].value;
                return value != null;
            }
        }

        value = entries[entries.Count - 1].value;
        return value != null;
    }
}
