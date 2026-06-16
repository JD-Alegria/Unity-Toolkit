# Input, Selection, And Dragging Guide

These systems are related but intentionally separate.

## Boundaries

- Input asks: what did the player do?
- Interaction asks: what object was activated?
- Selection asks: what object is currently selected?
- Dragging asks: what object is being moved by pointer input?

Do not merge these responsibilities into one manager.

## Pointer Interaction Setup

1. Add `ToolboxPlayerInputManager` to a scene object.
2. Assign `Primary Action`.
3. Optionally assign `Secondary Action`.
4. Assign `Input Camera`.
5. Configure `Interaction Layers` and `Raycast Distance`.
6. Put `IInteractable` on objects that should receive interactions.
7. Subscribe project scripts to pointer events for game-specific behavior.

Auto-interact can be enabled on press, hold, or release. Use this for simple objects like buttons, doors, or pickups.

For complex games, prefer subscribing to events and routing commands yourself.

## Button Commands

Use `buttonActions` for commands that do not need pointer raycasts, such as attack, cancel, rotate, or open menu.

Subscribe to:

- `ButtonPressed`
- `ButtonHeld`
- `ButtonReleased`

Keep button meaning in project code. The input manager should not know what "attack" means.

## Selection Setup

1. Add `SelectionManager` to a scene object.
2. Assign the scene's `ToolboxPlayerInputManager`.
3. Add `SelectableViewBinder` to simple selectable objects.
4. Add `GameObjectSelectionView` and assign the selection visual.
5. For complex units, implement `ISelectable` directly.

Project scripts can subscribe to:

- `SelectionChanged`
- `SelectionCleared`

Use this for RTS/tactics/management style games.

## Dragging Setup

1. Add `DragInputController` to an input object.
2. Assign drag action and camera.
3. Configure draggable and drop target layer masks.
4. Add `TransformDraggable` to movable objects.
5. Choose plane mode and allowed axes.
6. Add `SnapDropTarget` to slots or holders.

Use custom `IDraggable` when:

- object movement needs physics
- dragging changes game state
- drag should follow a path
- object should not directly move its transform

Use custom `IDropTarget` when:

- slots have occupancy
- only some draggables are accepted
- dropping should trigger validation or scoring

## Strengths

- Clear separation between pointer input, interaction, selection, and drag behavior.
- Project-specific logic can live in small scripts that subscribe to events.
- Reusable enough for first-person interaction, mouse management games, tabletop games, and tactics games.

## Weaknesses

- Mouse/pointer focused.
- No multi-select or drag-box selection.
- No touch ID support yet.
- No built-in hover/focus system.

## Recommended Next Improvements

- Add hover/focus events if multiple projects need them.
- Add multi-select as a separate module, not by bloating `SelectionManager`.
- Add touch support when a mobile project needs it.
