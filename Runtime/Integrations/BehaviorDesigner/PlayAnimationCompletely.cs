using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Jaleg.Toolkit;

// Custom Action Task assumes non-looping animation. Looping animation will increase normalizedTime endlessly
public class PlayAnimationCompletely : Action
{
    public SharedString animationName;
    
    Animator animator;
    bool hasEnteredState;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();

        if (animator == null) animator = gameObject.GetComponentInChildren<Animator>();
        if (animator == null) animator = gameObject.GetComponent<Animator>();
    }

    public override void OnStart()
    {
        hasEnteredState = false;
        
        if (animator == null)
        {
            Debug.LogError($"{nameof(PlayAnimationCompletely)} requires an Animator.");
        }
        
        animator.Play(animationName.Value, -1, 0f);
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null || string.IsNullOrWhiteSpace(animationName.Value)) return TaskStatus.Failure;

        if (animator.IsInTransition(0))
        {
            return TaskStatus.Running;
        }
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!hasEnteredState)
        {
            if (!stateInfo.IsName(animationName.Value))
            {
                return TaskStatus.Running;
            }
            hasEnteredState = true;
        }

        if (!stateInfo.IsName(animationName.Value))
        {
            return TaskStatus.Failure;
        }

        return stateInfo.normalizedTime < 1f ? TaskStatus.Running : TaskStatus.Success;
    }
}