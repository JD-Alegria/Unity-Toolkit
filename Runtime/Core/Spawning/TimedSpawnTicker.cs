using System;
using System.Collections;
using UnityEngine;

namespace Jaleg.Toolkit;

public class TimedSpawnTicker : MonoBehaviour
{
    [SerializeField] float initialDelay;
    [SerializeField] Vector2 intervalRange = new Vector2(1f, 1f);
    [SerializeField] bool playOnEnable;

    Coroutine routine;

    public event Action Tick;
    public bool IsRunning => routine != null;

    void OnEnable()
    {
        if (playOnEnable)
        {
            StartTicker();
        }
    }

    void OnDisable()
    {
        StopTicker();
    }

    public void StartTicker()
    {
        if (routine != null) return;

        routine = StartCoroutine(TickRoutine());
    }

    public void StopTicker()
    {
        if (routine == null) return;

        StopCoroutine(routine);
        routine = null;
    }

    IEnumerator TickRoutine()
    {
        if (initialDelay > 0f)
        {
            yield return new WaitForSeconds(initialDelay);
        }

        while (true)
        {
            Tick?.Invoke();
            yield return new WaitForSeconds(GetNextInterval());
        }
    }

    float GetNextInterval()
    {
        float min = Mathf.Min(intervalRange.x, intervalRange.y);
        float max = Mathf.Max(intervalRange.x, intervalRange.y);
        return Random.Range(min, max);
    }
}
