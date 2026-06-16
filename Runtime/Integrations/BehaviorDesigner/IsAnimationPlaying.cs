using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Jaleg.Toolkit;

// Behavior Designer conditional that checks whether a specific animator state
// is already active, or is currently being transitioned into.
// Useful in selector branches to avoid replaying an animation that is already running.
public class IsAnimationPlaying : Conditional
{
    Animator animator;

    public SharedString AnimationName;
    public SharedInt LayerIndex;

    public override void OnAwake()
    {
        if (animator == null) animator = gameObject.GetComponentInChildren<Animator>();
        if (animator == null) animator = gameObject.GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null || string.IsNullOrWhiteSpace(AnimationName.Value)) return TaskStatus.Failure;

        var currentState = animator.GetCurrentAnimatorStateInfo(LayerIndex.Value);
        if (currentState.IsName(AnimationName.Value)) return TaskStatus.Success;

        if (animator.IsInTransition(LayerIndex.Value))
        {
            var nextState = animator.GetNextAnimatorStateInfo(LayerIndex.Value);
            if (nextState.IsName(AnimationName.Value)) return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}