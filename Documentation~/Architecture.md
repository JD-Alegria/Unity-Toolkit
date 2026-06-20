# Architecture

The toolkit is split by reusable gameplay capability, not by Unity asset type.

## Runtime Rules

- Runtime code should compile without project scenes, sample data, or vendor demo assets.
- Core contracts should stay small and dependency-light.
- Third-party integrations belong under `Runtime/Integrations`.
- Project-specific examples belong under `Samples~`.
- Imported vendor assets belong under `ThirdParty~`, or should be installed directly in the consuming project.

## Module Boundaries

- `Core` owns stable contracts such as damage, interaction, and spawning.
- `Input` translates player input into toolkit interaction events.
- `Selection` is separate from interaction. Do not add selection visuals or state to `IInteractable`.
- `Dragging` builds on `Core` interaction contracts.
- `Combat` owns weapon firing, target detection, damage result data, and combat visuals.
- `Common` owns small presentation and utility helpers such as detached audio, camera-facing objects, weighted random selection, feedback relays, and looping object effects.
- `Animation` owns generic animation helpers.

## Reusable Versus Project Specific

Promote small primitives into the toolkit: contracts, context structs, result structs, detectors, spawn point pickers, weighted random utilities, and feedback relays.

Keep game-specific managers in game projects: fleet managers, battle event managers, scene unit registries, UI managers, and request factories with game-specific text or rules.

## Current Runtime Systems

- Input: `ToolboxPlayerInputManager`, `PointerInputContext`, and pointer phase data.
- Interaction: `IInteractable`, `IInteractor`, and `InteractionType`.
- Selection: `ISelectable`, `ISelectionView`, `SelectionManager`, `SelectableViewBinder`, and `GameObjectSelectionView`.
- Dragging: `IDraggable`, `IDropTarget`, `DragInputController`, `TransformDraggable`, and `SnapDropTarget`.
- Damage: `IDamageable` and `DamageInfo`.
- Combat: `HitscanShooter`, `HitscanResult`, `RangeTargetDetector`, `RangeTarget`, target filters, `VisionTargetDetector`, and weapon data/state helpers.
- Spawning: `Spawner`, `PrefabSpawner`, `SpawnPointPicker`, `TimedSpawnTicker`, and `ISpawnable`.
- Common helpers: `AudioDetachClip`, `FaceCamera`, `FeedbackRelay`, `LoopingObjectToggleEffect`, `VectorMovement`, and `WeightedRandom`.

## Optional Integrations

`Runtime/Integrations/BehaviorDesigner` is isolated in its own assembly. Add the scripting define symbol `BEHAVIOR_DESIGNER` in a project that has Behavior Designer installed to compile those tasks.
