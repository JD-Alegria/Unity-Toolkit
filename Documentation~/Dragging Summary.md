# Dragging Summary

## How It Works

`DragInputController` goes on an input object in the scene. Assign its drag action, drag camera, draggable layer mask, and drop target layer mask.

On mouse or button press, it raycasts for an `IDraggable`. While held, it calls `Drag()`. On release, it looks for an `IDropTarget`, then calls `EndDrag()`.

## Draggable Objects

`TransformDraggable` goes on any object you want to drag. In the inspector, choose:

- **Plane Mode:** What surface the pointer projects onto.
- **Reference:** Optional local-space reference, like your ticket table.
- **Allowed Axes:** Which local or world axes can move.
- **Preserve Grab Offset:** Whether the object keeps the exact spot you grabbed instead of snapping to the cursor.

## Ticket Setup

For tickets, the likely setup is:

- Add `TransformDraggable` to the ticket.
- Set **Plane Mode** to `TransformXZ`.
- Set **Reference** to the table or drag reference.
- Set **Allowed Axes** to `XZ`.
- Keep **Preserve Grab Offset** enabled.

## Guide Paper Or Flat Screen-Space Object

For a guide paper or flat screen-space object:

- Use `CameraFacing` or `WorldXY`.
- Use `XY` axes.
- Add custom behavior later by subclassing or replacing `TransformDraggable` with a project-specific component that still implements `IDraggable`.

## Drop Behavior

For drop behavior:

- Put `SnapDropTarget` on a slot or holder.
- Optionally assign a snap point.
- When a draggable is released over it, the target moves the object to that snap point.
