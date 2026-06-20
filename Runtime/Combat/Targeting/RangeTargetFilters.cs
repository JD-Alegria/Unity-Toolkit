using UnityEngine;
using Object = UnityEngine.Object;

namespace Jaleg.Toolkit;

public interface IRangeTargetFilter
{
    bool TryGetTarget(Collider collider, out GameObject targetObject, out Component targetComponent);
}

public sealed class AnyColliderRangeTargetFilter : IRangeTargetFilter
{
    public static readonly AnyColliderRangeTargetFilter Instance = new();

    AnyColliderRangeTargetFilter()
    {
    }

    public bool TryGetTarget(Collider collider, out GameObject targetObject, out Component targetComponent)
    {
        targetObject = collider != null ? collider.gameObject : null;
        targetComponent = collider;
        return targetObject != null && targetComponent != null;
    }
}

public sealed class DamageableRangeTargetFilter : IRangeTargetFilter
{
    public static readonly DamageableRangeTargetFilter Instance = new();

    DamageableRangeTargetFilter()
    {
    }

    public bool TryGetTarget(Collider collider, out GameObject targetObject, out Component targetComponent)
    {
        targetObject = null;
        targetComponent = null;

        if (collider == null) return false;

        if (!collider.TryGetComponent(out IDamageable damageable)) return false;
        if (damageable is not Component component) return false;
        if (component is not Object unityObject || unityObject == null) return false;
        if (!damageable.CanTakeDamage) return false;

        targetObject = damageable.GetGameObject();
        if (targetObject == null) return false;

        targetComponent = component;
        return true;
    }
}
