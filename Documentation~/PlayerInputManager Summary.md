# PlayerInputManager Summary

## How It Works

`ToolboxPlayerInputManager` reads a primary action, an optional secondary action, and optional non-pointer button actions. Each frame, pointer actions are checked for three phases: pressed, held, and released.

For each phase, it raycasts from the assigned camera through the mouse position, finds an `IInteractable`, builds a `PointerInputContext`, then fires events.

Button actions do not raycast. They emit `ButtonPressed`, `ButtonHeld`, and `ButtonReleased` so project code can bind commands like attack, cancel, rotate, or open menu without putting game-specific logic in the input component.

## Pointer Input Context

The context gives listeners useful info:

- `context.Phase`
- `context.InteractionType`
- `context.ScreenPosition`
- `context.PointerRay`
- `context.Hit`
- `context.WorldPoint`
- `context.Interactable`
- `context.Camera`
- `context.Interactor`

## Important Upgrades

- Uses `IInteractable` as the reusable contract.
- Separates press, hold, and release events.
- Supports primary and secondary interactions.
- Includes the interaction type in `PointerInputContext`.
- Supports generic non-pointer button events.
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
6. Add optional button actions for game commands that do not require raycasts.
7. Any object with a collider and `IInteractable` can now receive interactions.
