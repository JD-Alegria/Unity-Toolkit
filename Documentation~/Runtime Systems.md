# Runtime Systems

## Input And Interaction

`ToolboxPlayerInputManager` converts Input System actions into pointer and button events. Pointer events include `PointerInputContext`, which carries the interaction type, hit data, world point, ray, camera, interactor, and interactable.

Keep game-specific input behavior outside this class. Subscribe to its events from project scripts.

## Selection

Selection is separate from interaction.

- `ISelectable` describes things that can be selected.
- `ISelectionView` describes how selection is shown.
- `SelectionManager` listens to primary pointer presses and manages the selected object.
- `GameObjectSelectionView` toggles a selected visual.
- `SelectableViewBinder` connects a selectable object to a view.

Do not add selection indicators or selection state to `IInteractable`.

## Combat And Targeting

`HitscanShooter` executes hitscan fire and emits `HitscanResult` events for fired, hit, missed, and damage-applied outcomes. Ammo, reloads, input, and weapon visuals should stay in separate project or toolkit components.

`RangeTargetDetector` finds nearby targets by layer and optional filter, then reports both `CurrentTarget` and `AllTargets`. It owns range detection and priority selection, not attack decisions.

Default initialization treats the collider itself as the matched target component:

```csharp
detector.Init(targetingData);
RangeTarget target = detector.CurrentTarget;
```

Combat initialization supplies `DamageableRangeTargetFilter`, which only accepts components implementing `IDamageable` with `CanTakeDamage == true`:

```csharp
detector.Init(targetingData, DamageableRangeTargetFilter.Instance);
detector.CurrentTarget.TryGetTarget(out IDamageable damageable);
```

## Spawning

The toolkit avoids project-level spawn singletons. Use small pieces instead:

- `SpawnPointPicker` chooses a spawn pose.
- `PrefabSpawner` instantiates a prefab and emits `Spawned`.
- `TimedSpawnTicker` emits timed ticks for wave or request logic.
- `WeightedRandom` handles weighted selection.
- `Spawner` provides direct spawn helper methods.

Game-specific spawn rules belong in the game project.

## Feedback And Presentation

Gameplay components should emit events. Feedback scripts should subscribe and play audio, VFX, animation, UI, or third-party feedback tools.

Useful helpers:

- `FeedbackRelay`
- `AudioDetachClip`
- `FaceCamera`
- `LoopingObjectToggleEffect`
- `BulletTracer`
