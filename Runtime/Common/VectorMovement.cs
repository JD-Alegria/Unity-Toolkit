using System.Collections;
using UnityEngine;

namespace Jaleg.Toolkit;

public class VectorMovement : MonoBehaviour
{
    Transform targetPos;
    float movementSpeed = 3f;
    float rotationSpeed = 1f;

    public void Init<TData>(TData data)
    {
        
    }
    
    IEnumerator LerpSpeed(float startSpeed, float endSpeed,float duration = 2.5f)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            movementSpeed = Mathf.Lerp(startSpeed, endSpeed, t);
            
            yield return null;
        }
        
        movementSpeed = endSpeed;
    }
}
