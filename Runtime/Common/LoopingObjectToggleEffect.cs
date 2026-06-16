using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jaleg.Toolkit;

public class LoopingObjectToggleEffect : MonoBehaviour
{
    [SerializeField] List<GameObject> targets = new();
    [SerializeField] float interval = 0.5f;
    [SerializeField] bool playOnEnable;
    [SerializeField] bool turnOffWhenStopped = true;

    Coroutine routine;

    public bool IsPlaying => routine != null;

    void OnEnable()
    {
        if (playOnEnable)
        {
            Play();
        }
    }

    void OnDisable()
    {
        Stop();
    }

    public void Play()
    {
        if (routine != null) return;

        routine = StartCoroutine(ToggleRoutine());
    }

    public void Stop()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        if (turnOffWhenStopped)
        {
            SetTargetsActive(false);
        }
    }

    IEnumerator ToggleRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(interval);

        while (true)
        {
            ToggleTargets();
            yield return wait;
        }
    }

    void ToggleTargets()
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                target.SetActive(!target.activeSelf);
            }
        }
    }

    void SetTargetsActive(bool active)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                target.SetActive(active);
            }
        }
    }
}
