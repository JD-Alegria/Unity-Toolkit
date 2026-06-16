using UnityEngine;
using System.Collections;

namespace Jaleg.Toolkit;

public class BulletTracer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float speed = 250f;
    [SerializeField] float tracerLength = 0.75f;
    [SerializeField] bool destroyWhenComplete = true;

    public void Initialize(Vector3 start, Vector3 end)
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        if (lineRenderer == null)
        {
            Debug.LogWarning($"{nameof(BulletTracer)} requires a LineRenderer.", this);
            return;
        }

        StartCoroutine(AnimateTracer(start, end));
    }

    IEnumerator AnimateTracer(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        float traveled = 0f;
        Vector3 direction = (end - start).normalized;

        while (traveled < distance)
        {
            traveled += speed * Time.deltaTime;
            
            Vector3 head = start + direction * traveled;
            Vector3 tail = start + direction * Mathf.Max(0f, traveled - tracerLength);

            lineRenderer.SetPosition(0, tail);
            lineRenderer.SetPosition(1, head);
            
            yield return null;
        }
        
        lineRenderer.SetPosition(0, end);
        lineRenderer.SetPosition(1, end);

        if (destroyWhenComplete)
        {
            Destroy(gameObject);
        }
    }
}
