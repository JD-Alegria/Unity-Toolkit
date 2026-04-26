using UnityEngine;
using System.Collections;

// make gameObject scale tiny = 
public class BulletTracer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float speed = 250f;
    [SerializeField] float tracerLength = 0.75f;

    public void Initialize(Vector3 start, Vector3 end)
    {
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

        Destroy(gameObject);
    }
}