# PlayerInputManager Summary

## How It Works

`ToolboxPlayerInputManager` reads a primary and optional secondary `InputActionReference`. Each frame, it checks for three phases: pressed, held, and released.

For each phase, it raycasts from the assigned camera through the mouse position, finds an `IInteractable`, builds a `PointerInputContext`, then fires events.

## Pointer Input Context

The context gives listeners useful info:

- `context.Phase`
- `context.ScreenPosition`
- `context.PointerRay`
- `context.Hit`
- `context.Interactable`
- `context.Camera`
- `context.Interactor`

## Important Upgrades

- Uses `IInteractable` as the reusable contract.
- Separates press, hold, and release events.
- Supports primary and secondary interactions.
- Uses layer masks, raycast distance, and trigger settings.
- Can ignore UI clicks.
- Can search parent objects for `IInteractable`, which is useful when colliders live on child objects.
- Supports optional auto-interact on press, hold, or release through inspector toggles.
- Exposes generic events like `InteractablePressed`, `InteractableHeld`, and `InteractableReleased`.

## Basic Flow

1. Add `ToolboxPlayerInputManager` to a scene object.
2. Assign **Primary Action**, usually your click action.
3. Assign **Input Camera**.
4. Set **Interaction Layers**.
5. Choose whether interaction happens on press, hold, or release.
6. Any object with a collider and `IInteractable` can now receive interactions.
