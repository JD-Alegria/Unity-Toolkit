# Spawning And Common Utilities Guide

This toolkit avoids reusable singleton managers. Instead, it provides small utilities that game-specific managers can compose.

## Spawning Philosophy

Do not promote a project `SpawnManager` directly into the toolkit. Extract the reusable pieces:

- picking spawn points
- instantiating prefabs
- emitting spawn events
- timing repeated spawns
- weighted random selection

The game project should own caps, waves, difficulty, request rules, story rules, and object registration.

## Basic Prefab Spawn

1. Add `PrefabSpawner` to a scene object.
2. Assign a prefab.
3. Optionally assign `SpawnPointPicker`.
4. Call `Spawn()` from project code.
5. Subscribe to `Spawned` for registration, audio, UI, or follow-up setup.

## Spawn Point Picking

`SpawnPointPicker` chooses a random transform from a list and applies optional random offset.

Use it for:

- enemy spawn points
- ship entry points
- request/item spawn locations
- pickups
- wave systems

It does not handle occupancy, navmesh validation, or weighting. Add those in project code or future focused utilities.

## Timed Spawning

`TimedSpawnTicker` emits `Tick` on a loop.

Use it when a project manager needs timing but should own the actual rules.

Example:

```csharp
void OnEnable()
{
    ticker.Tick += TrySpawnEnemy;
}

void TrySpawnEnemy()
{
    if (activeEnemies >= cap) return;
    spawner.Spawn();
}
```

## Weighted Random

`WeightedRandom` handles common weighted selection.

Use it for:

- enemy type selection
- ship type selection
- loot choice
- event choice
- request variants

Do not put game-specific random text generation into the toolkit. Keep that in project factories.

## Common Presentation Helpers

### `AudioDetachClip`

Use when an object needs to play a clip even if the source object is destroyed, despawned, or moved.

Weakness: creates a temporary GameObject. Use pooling later if this becomes hot.

### `FaceCamera`

Use for world-space labels, unit indicators, health bars, and floating UI.

Weakness: some UI orientations may need a custom billboard variant.

### `FeedbackRelay`

Use as an inspector-configured bridge to `UnityEvent`.

Good for connecting generic toolkit/project events to simple feedback without writing a new script.

Weakness: no typed payload.

### `LoopingObjectToggleEffect`

Use for simple blinking/flickering objects.

Good for:

- muzzle flashes
- alert lights
- warning indicators
- temporary firing visuals

Weakness: simple active-state toggling only.

## Strengths

- Keeps reusable spawn logic small.
- Avoids global singleton dependencies.
- Makes project managers thinner.
- Encourages event-driven registration and feedback.

## Weaknesses

- No object pooling yet.
- No spawn cap manager.
- No weighted prefab spawner component.
- No navmesh-safe spawn picker.

## Recommended Next Improvements

- Add pooling only after at least one project clearly needs it.
- Add weighted prefab spawning as a small component if repeated.
- Add spawn validation strategies if navmesh/occupancy logic repeats.
