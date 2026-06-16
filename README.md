# Unity Toolkit

Reusable Unity gameplay code for future projects.

This repository is organized as a Unity Package Manager package. Add it to another Unity project with a local file dependency:

```json
"com.jaleg.unity-toolkit": "file:../Unity Toolkit"
```

## Layout

- `Runtime/Core`: contracts and small shared types.
- `Runtime/Input`: pointer and interaction input components.
- `Runtime/Dragging`: drag-and-drop components.
- `Runtime/Combat`: weapon, damage, targeting, and combat helpers.
- `Runtime/Core/Selection`: selection contracts and a reusable selection manager.
- `Runtime/Core/Spawning`: prefab spawning, spawn point picking, and timed spawn ticks.
- `Runtime/Common`: small utility and presentation helpers.
- `Runtime/Animation`: generic animation helpers.
- `Runtime/Integrations`: optional code that depends on third-party assets.
- `Samples~`: examples and project-specific reference code.
- `ThirdParty~`: imported vendor assets kept for reference, outside package runtime.
- `Documentation~`: design notes and usage docs.

Keep runtime code general. Put game-specific examples, prefabs, and experimental code in `Samples~`.

## Documentation

- `Documentation~/Architecture.md`: package-level architecture rules and module boundaries.
- `Documentation~/Runtime Systems.md`: overview of each runtime system.
- `Documentation~/Script Review Guide.md`: script-by-script review with role, usage, strengths, and weaknesses.
- `Documentation~/Combat System Guide.md`: combat/targeting implementation flow.
- `Documentation~/Input Selection Dragging Guide.md`: interaction, selection, and drag workflow.
- `Documentation~/Spawning And Common Utilities Guide.md`: spawning, random selection, and presentation helpers.

## Architecture Bias

The toolkit provides small gameplay primitives rather than project-level managers. Prefer composing these from game-specific scripts over promoting a whole `GameManager`, `SpawnManager`, or `AudioManager` into the package.

Core reusable seams include input context, interaction contracts, selection contracts, damage payloads, hitscan results, target detectors, spawn utilities, weighted random selection, and event-driven feedback helpers.
