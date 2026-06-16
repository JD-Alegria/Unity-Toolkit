# Script Review Guide

This document is for reviewing the toolkit one script at a time. Each entry explains what the script owns, how to use it, how it connects to the larger system, and what still needs work.

## Core Damage

### `IDamageable`

Role: contract for anything that can receive damage.

Use it on health components, ship hulls, enemy hitboxes, player damage receivers, destructible props, or tactical units.

Important members:

- `CanTakeDamage`: lets detectors and shooters ignore invulnerable or inactive targets.
- `ApplyDamage(in DamageInfo damageInfo)`: receives damage payloads.
- `GetGameObject()`: lets generic systems locate the target transform without knowing the concrete component type.

Works with:

- `HitscanShooter`
- `RangeTargetDetector`
- `VisionTargetDetector`
- project-specific attack controllers

Strengths:

- Small and reusable.
- Decouples weapon code from specific enemy/player/ship classes.
- `DamageInfo` can carry context without changing every weapon method signature.

Weaknesses:

- `GetGameObject()` is practical but slightly Unity-specific.
- `CanTakeDamage` is read-only, so project code must expose state through its own component.
- No built-in team/faction filtering yet.

Recommended pattern:

```csharp
public class EnemyHealth : MonoBehaviour, IDamageable
{
    public bool CanTakeDamage => currentHealth > 0;

    public void ApplyDamage(in DamageInfo damageInfo)
    {
        currentHealth -= damageInfo.Amount;
    }

    public GameObject GetGameObject() => gameObject;
}
```

### `DamageInfo`

Role: immutable payload describing damage.

Carries:

- amount
- source object
- hit point
- hit normal
- optional damage type string

Strengths:

- Good middle ground between simple integer damage and overbuilt damage systems.
- Works for bullets, ship attacks, melee, traps, and environmental damage.

Weaknesses:

- `DamageType` is a string. That is flexible but easy to mistype.
- No explicit attacker/team/faction metadata.

Future upgrade:

- Add an optional strongly typed damage category or source interface if multiple games need it.

## Core Interaction

### `IInteractable`

Role: contract for anything that can be interacted with.

Use it on doors, pickups, switches, selectable objects, cards, tickets, ships, panels, or world objects.

Important members:

- `CanInteract(IInteractor interactor)`
- `Interact(IInteractor interactor, InteractionType interaction)`

Strengths:

- Avoids coupling to a concrete player class.
- Supports primary and secondary interaction.
- Keeps input code generic.

Weaknesses:

- Does not define hover/focus state.
- Does not define selection state, intentionally.

Correct boundary:

- Interaction answers: "What happens when something activates this?"
- Selection answers: "Is this currently selected?"
- Dragging answers: "Can the pointer move this?"

### `IInteractor`

Role: represents the actor causing interaction.

Usually implemented by:

- `ToolboxPlayerInputManager`
- `DragInputController`
- `SelectionManager`
- project-specific AI or controller scripts

Strengths:

- Lets interactables ask who interacted.
- Keeps ownership generic through `Owner` and `Origin`.

Weaknesses:

- No team/faction/user metadata yet.

### `InteractionType`

Role: distinguishes command intent.

Current values:

- `Primary`
- `Secondary`

Use examples:

- Primary click selects or activates.
- Secondary click orders movement.
- Primary drag picks up.
- Secondary action opens alternate behavior.

## Input

### `ToolboxPlayerInputManager`

Role: converts Input System actions into reusable pointer and button events.

Owns:

- enabling/disabling input actions
- pointer raycasts
- UI click ignoring
- interactable lookup
- pointer phase events
- optional auto-interact
- generic non-pointer button events

Does not own:

- game commands
- selected unit behavior
- attack decisions
- object-specific type checks

Important events:

- `PointerPressed`
- `PointerHeld`
- `PointerReleased`
- `PointerPressedAway`
- `PointerReleasedAway`
- `ButtonPressed`
- `ButtonHeld`
- `ButtonReleased`
- `InteractablePressed`
- `InteractableHeld`
- `InteractableReleased`

Strengths:

- Replaces several project-specific `PlayerInputManager` variants.
- Keeps game-specific logic in listeners instead of input code.
- Supports both pointer interaction and generic button commands.

Weaknesses:

- Currently mouse-focused.
- Does not yet support screen touch IDs or gamepad cursor workflows.
- Only does one raycast path.

Implementation checklist:

1. Add `ToolboxPlayerInputManager` to a scene object.
2. Assign primary and optional secondary `InputActionReference`.
3. Assign camera, interaction layer mask, and raycast distance.
4. Subscribe project scripts to events.
5. Keep type-specific behavior out of this component.

### `PointerInputContext`

Role: immutable event payload for pointer actions.

Carries:

- interactor
- interaction type
- camera
- phase
- screen position
- pointer ray
- optional raycast hit
- interactable
- world point convenience property

Strengths:

- Gives listeners enough information to make decisions without re-raycasting.
- Prevents input code from needing to know game rules.

Weaknesses:

- `WorldPoint` returns default when nothing was hit, so listeners should check `HitSomething`.

### `PointerInputPhase`

Role: identifies pointer timing.

Values:

- `Pressed`
- `Held`
- `Released`

## Selection

### `ISelectable`

Role: contract for objects that can be selected.

Important members:

- `CanSelect`
- `Select`
- `Deselect`
- `Owner`

Strengths:

- Keeps selection separate from interaction.
- Avoids adding UI/indicator properties to `IInteractable`.

Weaknesses:

- Does not define multi-select yet.
- Does not define hover preview yet.

### `ISelectionView`

Role: presentation contract for selection visuals.

Use it for:

- selection rings
- outline effects
- floating UI
- highlight meshes

Strengths:

- Lets selection state and visual representation stay separate.

### `SelectionManager`

Role: listens to pointer input and manages one selected object.

Owns:

- current selected object
- selecting on primary pointer press
- clearing selection when pressing away
- selection changed events

Does not own:

- movement orders
- attack orders
- UI command panels

Strengths:

- Good base for RTS/tactics/management games.
- Does not require selected objects to also be interactables.

Weaknesses:

- Single-selection only.
- No drag-box selection yet.
- No command routing beyond selection.

Implementation checklist:

1. Add `SelectionManager` to a scene object.
2. Assign a `ToolboxPlayerInputManager`.
3. Add `ISelectable` to selectable objects, either custom or via `SelectableViewBinder`.
4. Subscribe command scripts to `SelectionChanged`.

### `SelectableViewBinder`

Role: simple selectable component that delegates visuals to an `ISelectionView`.

Strengths:

- Lets simple objects become selectable without writing a custom selectable class.

Weaknesses:

- Uses a serialized `MonoBehaviour` reference that must implement `ISelectionView`.
- Not ideal for complex unit logic; write a custom `ISelectable` for that.

### `GameObjectSelectionView`

Role: toggles a selected visual GameObject.

Strengths:

- Good default for selection rings and simple highlights.

Weaknesses:

- Only toggles one object. More complex effects should implement `ISelectionView`.

## Dragging

### `IDraggable`, `IDropTarget`, `DragContext`

Role: contracts and context for pointer dragging.

Strengths:

- Separates input, draggable behavior, and drop behavior.
- Reusable across tickets, cards, inventory items, tabletop objects, and world objects.

Weaknesses:

- Current drag flow is pointer/mouse oriented.
- Drop target selection is simple raycast-based.

### `DragInputController`

Role: pointer input controller specifically for drag interactions.

Owns:

- starting drag
- updating active draggable
- finding drop target on release

Strengths:

- Keeps drag behavior out of general interaction input.

Weaknesses:

- Can overlap conceptually with `ToolboxPlayerInputManager`; use one or the other per workflow.

### `DragPlaneSettings`

Role: inspector-friendly settings for projection plane and movement axes.

Use it to constrain drag movement to:

- camera-facing plane
- world XY/XZ
- transform-local XY/XZ

### `TransformDraggable`

Role: default implementation for moving a transform by pointer drag.

Strengths:

- Supports local/world plane choices.
- Preserves grab offset.
- Emits drag events.

Weaknesses:

- It directly moves transform position. Physics-based dragging should use a custom `IDraggable`.

### `SnapDropTarget`

Role: default drop target that snaps released draggable objects to a transform.

Strengths:

- Good for slots, holders, docks, sockets, and board-game-style placement.

Weaknesses:

- No occupancy rules by default. Add those in a custom `IDropTarget`.

## Combat

### `IHitscanShooterConfig`

Role: config contract for hitscan firing.

Strengths:

- Lets project-specific ScriptableObjects or components configure `HitscanShooter`.
- Avoids hard dependency on `WeaponData`.

Weaknesses:

- `hitscanOrigin` should eventually be renamed to `HitscanOrigin` for C# property style.

### `HitscanShooter`

Role: executes hitscan weapon fire.

Owns:

- fire rate cooldown
- straight/spread/spherecast cast
- damage application through `IDamageable`
- shot result events

Does not own:

- player input
- ammo
- reload
- muzzle flash
- audio
- team filtering

Strengths:

- Emits `HitscanResult`, which keeps VFX/audio/UI separate.
- Supports hit, miss, and damage-applied events.
- Can be used by player weapons, turrets, AI, or ships.

Weaknesses:

- Spread is simple pitch/yaw randomization.
- No built-in team/faction filtering.
- No built-in ammo/reload integration.

Implementation checklist:

1. Create a data object or component implementing `IHitscanShooterConfig`.
2. Add `HitscanShooter` to the weapon object.
3. Call `Init(config)` before firing.
4. Call `TryFire(out HitscanResult result)` from project input/AI code.
5. Subscribe presentation scripts to `Fired`, `Hit`, `Missed`, or `DamageApplied`.

### `HitscanResult`

Role: immutable shot result payload.

Carries:

- did fire
- did hit
- did apply damage
- origin
- direction
- endpoint
- optional raycast hit
- optional damageable

Strengths:

- Lets one shot feed tracer, impact, audio, camera shake, UI, and logs.

### `RangeTargetDetector`

Role: finds nearby `IDamageable` targets.

Owns:

- overlap sphere detection
- target filtering
- closest/farthest priority selection
- target-changed and targets-updated events

Does not own:

- deciding to attack
- movement
- faction rules
- damage

Strengths:

- Reports both primary target and all valid targets.
- Good for turrets, AI, strike groups, and tactical overlays.

Weaknesses:

- Cone detection is not implemented yet.
- Uses `IDamageable.CanTakeDamage` but not team/faction filtering.

### `VisionTargetDetector`

Role: line-of-sight style target checks.

Strengths:

- Useful idea for "can see target" checks.

Weaknesses:

- Less mature than `RangeTargetDetector`.
- Some optional parameters are not used.
- Overlaps with hitscan cast logic.

Recommendation:

- Treat as experimental until it is refactored around shared cast result logic.

### `WeaponData`

Role: default ScriptableObject for weapon tuning.

Strengths:

- Useful starter data for conventional ranged weapons.
- Combines display, combat, ammo, and audio fields.

Weaknesses:

- More opinionated than the config interfaces.
- Not every game should use it directly.

Recommendation:

- Use it when it fits. Otherwise make project-specific data that implements the toolkit interfaces.

### `RangedWeaponState`

Role: intended mutable runtime weapon state.

Strengths:

- Correct idea: separate mutable state from static data.

Weaknesses:

- Currently underdeveloped.
- Missing public state accessors.
- Does not initialize every copied field.

Recommendation:

- Treat as a placeholder until ammo/reload architecture is clarified.

### `WeaponModelController`

Role: intended weapon model swap/spawn helper.

Strengths:

- Correct separation from firing logic.

Weaknesses:

- Empty placeholder.

Recommendation:

- Fill when a second project needs reusable weapon model swapping, or remove if it stays unused.

### `BulletTracer`

Role: visual tracer using a `LineRenderer`.

Strengths:

- Simple presentation component.
- Works well with `HitscanResult.Origin` and `HitscanResult.EndPoint`.

Weaknesses:

- Visual style is minimal.
- Does not pool instances.

Implementation example:

```csharp
shooter.Fired += result =>
{
    BulletTracer tracer = Instantiate(tracerPrefab).GetComponent<BulletTracer>();
    tracer.Initialize(result.Origin, result.EndPoint);
};
```

## Spawning

### `ISpawnable`

Role: generic initialization contract for spawned objects.

Strengths:

- Lets spawned prefabs receive ScriptableObject data without the spawner knowing concrete types.

Weaknesses:

- ScriptableObject constraint may be too narrow for some runtime-only data.

### `Spawner`

Role: direct spawn helper.

Strengths:

- Small and generic.
- Can initialize `ISpawnable<TData>`.

Weaknesses:

- Thin helper; most projects may prefer `PrefabSpawner` or custom orchestration.

### `PrefabSpawner`

Role: component that spawns a configured prefab.

Strengths:

- Emits `Spawned`, allowing UI/audio/registries to respond.
- Can use `SpawnPointPicker`.

Weaknesses:

- Only one prefab. Weighted prefab choice should be composed with `WeightedRandom`.

### `SpawnPointPicker`

Role: chooses a spawn pose from a list of transforms with optional random offset.

Strengths:

- Reusable foundation for waves, enemy spawns, ship arrivals, and request objects.

Weaknesses:

- No weighting or occupancy checks.

### `TimedSpawnTicker`

Role: emits repeated timed ticks.

Strengths:

- Extracts timing from game-specific spawn managers.
- Good for waves, request generation, ambient events, or timed hazards.

Weaknesses:

- Does not track caps, difficulty, or active spawned objects.

## Common Helpers

### `WeightedRandom`

Role: static weighted selection utility.

Strengths:

- Extracts repeated random choice logic.
- Useful for enemy types, ship types, events, loot, and request variants.

Weaknesses:

- Runtime-only helper; no custom inspector.

### `AudioDetachClip`

Role: plays a temporary detached `AudioSource`.

Strengths:

- Useful when the source object may be destroyed or move away.

Weaknesses:

- Allocates a temporary GameObject.
- Does not pool audio sources.

### `FaceCamera`

Role: makes an object face a camera in `LateUpdate`.

Strengths:

- Good for world-space UI, indicators, health bars, and labels.

Weaknesses:

- Uses forward matching only. Some UI setups may need inverse forward or billboard constraints.

### `FeedbackRelay`

Role: invokes a serialized `UnityEvent`.

Strengths:

- Simple bridge between gameplay events and inspector-configured feedback.

Weaknesses:

- No typed payload support.

### `LoopingObjectToggleEffect`

Role: repeatedly toggles a list of GameObjects.

Strengths:

- Useful for simple muzzle flicker, warning lights, alert markers, and temporary effects.

Weaknesses:

- Simple visual behavior only.
- More complex effects should use animation, particle systems, or feedback tools.

### `VectorMovement`

Role: early movement helper.

Strengths:

- Has the seed of reusable speed interpolation.

Weaknesses:

- Currently incomplete.
- No clear public movement API yet.

Recommendation:

- Treat as experimental until movement utilities are designed from a second or third repeated use case.

## Optional Integrations

### `IsAnimationPlaying`

Role: Behavior Designer conditional for checking animator state.

Strengths:

- Useful for behavior tree animation guards.

Weaknesses:

- Requires Behavior Designer and the `BEHAVIOR_DESIGNER` scripting define.

### `PlayAnimationCompletely`

Role: Behavior Designer action that plays an animation and returns success when complete.

Strengths:

- Encapsulates common behavior tree animation timing.

Weaknesses:

- Assumes non-looping animation.
- Requires Behavior Designer and the `BEHAVIOR_DESIGNER` scripting define.
