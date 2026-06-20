# Combat System Guide

The combat system is built from small tools. A project script should compose these tools into a weapon, turret, enemy, or unit behavior.

## Core Idea

The toolkit separates four jobs:

- Detect targets.
- Decide when/what to attack.
- Execute the hit or shot.
- Present feedback.

The toolkit handles detection and generic execution. Your game project owns decisions and feedback composition.

## Basic Hitscan Weapon

1. Create a config source that implements `IHitscanShooterConfig`.
2. Add `HitscanShooter` to the weapon object.
3. Call `Init(config)` before firing.
4. Call `TryFire(out HitscanResult result)` from player input or AI code.
5. Subscribe feedback scripts to `Fired`, `Hit`, `Missed`, or `DamageApplied`.

Example flow:

```csharp
public class PlayerGun : MonoBehaviour
{
    [SerializeField] HitscanShooter shooter;
    [SerializeField] WeaponConfig weaponConfig;

    void Awake()
    {
        shooter.Init(weaponConfig);
        shooter.Fired += HandleFired;
    }

    public void Fire()
    {
        shooter.TryFire(out _);
    }

    void HandleFired(HitscanResult result)
    {
        Debug.DrawLine(result.Origin, result.EndPoint, Color.red, 0.1f);
    }
}
```

## Targeted Turret

Use `RangeTargetDetector` to find a target, then a project-specific turret script rotates and fires.

```text
TargetingData implements ITargetDetectorConfig
RangeTargetDetector.Init(TargetingData, DamageableRangeTargetFilter.Instance)
TurretController reads CurrentTarget
TurretController gets IDamageable from CurrentTarget.TargetComponent
TurretController rotates model
TurretWeapon calls HitscanShooter.TryFire()
Target receives IDamageable.ApplyDamage()
```

`RangeTargetDetector` should not rotate the turret or decide hostile teams. It only detects. The filter decides which collider counts as a valid target for this detector.

Combat target example:

```csharp
detector.Init(targetingData, DamageableRangeTargetFilter.Instance);

RangeTarget target = detector.CurrentTarget;
if (target.TryGetTarget(out IDamageable damageable))
{
    damageable.ApplyDamage(damageInfo);
}
```

Non-combat object target example:

```csharp
detector.Init(targetingData);

RangeTarget target = detector.CurrentTarget;
GameObject targetObject = target.GameObject;
```

## Strategy Unit Attack

For a Condition I style attack:

```text
SelectionManager selects unit
Project command script receives attack button
RangeTargetDetector finds targets
AttackController picks CurrentTarget
Movement script moves toward target
AttackController applies damage or calls HitscanShooter
ShipEffect/FeedbackRelay handles visuals
```

This keeps toolkit systems reusable and lets the game project own the rules.

## Combat Strengths

- `IDamageable` keeps damage generic.
- `DamageInfo` carries context.
- `HitscanResult` separates weapon execution from feedback.
- `RangeTargetDetector` reports both primary and all valid targets through `RangeTarget`.
- `DamageableRangeTargetFilter` keeps damage-specific validation outside the general range detector.
- Config interfaces let project-specific ScriptableObjects drive toolkit components.

## Combat Weaknesses

- No built-in faction/team filtering.
- `VisionTargetDetector` is less mature than `RangeTargetDetector`.
- `RangedWeaponState` and `WeaponModelController` are placeholders.
- No pooled tracer/audio helpers yet.
- Cone detection is not implemented.

## Recommended Next Improvements

- Add `ITeamProvider` or `ITargetFilter` only after multiple projects need it.
- Refactor `VisionTargetDetector` around shared cast result logic.
- Build a real ammo/reload state component if a second game repeats the need.
- Add pooling hooks for `BulletTracer`.
