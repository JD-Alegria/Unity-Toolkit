using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Jaleg.Toolkit;

public class ToolboxPlayerInputManager : MonoBehaviour, IInteractor
{
    [Header("Input")]
    [SerializeField] InputActionReference primaryAction;
    [SerializeField] InputActionReference secondaryAction;
    [SerializeField] InputActionReference[] buttonActions;

    [Header("Raycast")]
    [SerializeField] Camera inputCamera;
    [SerializeField] LayerMask interactionLayers = -1;
    [SerializeField] float raycastDistance = 10000f;
    [SerializeField] QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Collide;
    [SerializeField] bool searchParentsForInteractable = true;
    [SerializeField] bool ignoreUi = true;

    [Header("Interaction")]
    [SerializeField] bool interactOnPress = true;
    [SerializeField] bool interactOnHold;
    [SerializeField] bool interactOnRelease;

    public event Action<PointerInputContext> PointerPressed;
    public event Action<PointerInputContext> PointerHeld;
    public event Action<PointerInputContext> PointerReleased;
    public event Action<PointerInputContext> PointerPressedAway;
    public event Action<PointerInputContext> PointerReleasedAway;
    public event Action<InputActionReference> ButtonPressed;
    public event Action<InputActionReference> ButtonHeld;
    public event Action<InputActionReference> ButtonReleased;
    public event Action<IInteractable, PointerInputContext> InteractablePressed;
    public event Action<IInteractable, PointerInputContext> InteractableHeld;
    public event Action<IInteractable, PointerInputContext> InteractableReleased;

    public GameObject Owner => gameObject;
    public Transform Origin => transform;

    void OnEnable()
    {
        EnableAction(primaryAction);
        EnableAction(secondaryAction);
        EnableActions(buttonActions);
    }

    void OnDisable()
    {
        DisableAction(primaryAction);
        DisableAction(secondaryAction);
        DisableActions(buttonActions);
    }

    void Update()
    {
        ProcessButtonActions(buttonActions);

        if (primaryAction == null || inputCamera == null || Mouse.current == null) return;
        if (ignoreUi && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        ProcessAction(primaryAction, InteractionType.Primary);

        if (secondaryAction != null)
        {
            ProcessAction(secondaryAction, InteractionType.Secondary);
        }
    }

    void ProcessAction(InputActionReference actionReference, InteractionType interactionType)
    {
        if (actionReference == null) return;

        InputAction action = actionReference.action;

        if (action.WasPressedThisFrame())
        {
            PointerInputContext context = CreateContext(PointerInputPhase.Pressed, interactionType);
            HandlePressed(context, interactionType);
        }

        if (action.IsPressed())
        {
            PointerInputContext context = CreateContext(PointerInputPhase.Held, interactionType);
            HandleHeld(context, interactionType);
        }

        if (action.WasReleasedThisFrame())
        {
            PointerInputContext context = CreateContext(PointerInputPhase.Released, interactionType);
            HandleReleased(context, interactionType);
        }
    }

    void ProcessButtonActions(InputActionReference[] actions)
    {
        if (actions == null) return;

        foreach (InputActionReference actionReference in actions)
        {
            ProcessButtonAction(actionReference);
        }
    }

    void ProcessButtonAction(InputActionReference actionReference)
    {
        if (actionReference == null) return;

        InputAction action = actionReference.action;

        if (action.WasPressedThisFrame())
        {
            ButtonPressed?.Invoke(actionReference);
        }

        if (action.IsPressed())
        {
            ButtonHeld?.Invoke(actionReference);
        }

        if (action.WasReleasedThisFrame())
        {
            ButtonReleased?.Invoke(actionReference);
        }
    }

    void HandlePressed(PointerInputContext context, InteractionType interactionType)
    {
        PointerPressed?.Invoke(context);

        if (context.Interactable == null)
        {
            PointerPressedAway?.Invoke(context);
            return;
        }

        InteractablePressed?.Invoke(context.Interactable, context);

        if (interactOnPress)
        {
            TryInteract(context.Interactable, interactionType);
        }
    }

    void HandleHeld(PointerInputContext context, InteractionType interactionType)
    {
        PointerHeld?.Invoke(context);

        if (context.Interactable == null) return;

        InteractableHeld?.Invoke(context.Interactable, context);

        if (interactOnHold)
        {
            TryInteract(context.Interactable, interactionType);
        }
    }

    void HandleReleased(PointerInputContext context, InteractionType interactionType)
    {
        PointerReleased?.Invoke(context);

        if (context.Interactable == null)
        {
            PointerReleasedAway?.Invoke(context);
            return;
        }

        InteractableReleased?.Invoke(context.Interactable, context);

        if (interactOnRelease)
        {
            TryInteract(context.Interactable, interactionType);
        }
    }

    void TryInteract(IInteractable interactable, InteractionType interactionType)
    {
        if (!interactable.CanInteract(this)) return;

        interactable.Interact(this, interactionType);
    }

    PointerInputContext CreateContext(PointerInputPhase phase, InteractionType interactionType)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Ray ray = inputCamera.ScreenPointToRay(screenPosition);
        RaycastHit? hit = TryGetHit(ray, out RaycastHit raycastHit) ? raycastHit : null;
        IInteractable interactable = hit.HasValue ? GetInteractable(hit.Value.collider) : null;

        return new PointerInputContext(
            this,
            interactionType,
            inputCamera,
            phase,
            screenPosition,
            ray,
            hit,
            interactable);
    }

    bool TryGetHit(Ray ray, out RaycastHit hit)
    {
        return Physics.Raycast(
            ray,
            out hit,
            raycastDistance,
            interactionLayers,
            triggerInteraction);
    }

    IInteractable GetInteractable(Collider hitCollider)
    {
        if (searchParentsForInteractable)
        {
            return hitCollider.GetComponentInParent<IInteractable>();
        }

        return hitCollider.GetComponent<IInteractable>();
    }

    static void EnableAction(InputActionReference actionReference)
    {
        if (actionReference != null)
        {
            actionReference.action.Enable();
        }
    }

    static void DisableAction(InputActionReference actionReference)
    {
        if (actionReference != null)
        {
            actionReference.action.Disable();
        }
    }

    static void EnableActions(InputActionReference[] actions)
    {
        if (actions == null) return;

        foreach (InputActionReference actionReference in actions)
        {
            EnableAction(actionReference);
        }
    }

    static void DisableActions(InputActionReference[] actions)
    {
        if (actions == null) return;

        foreach (InputActionReference actionReference in actions)
        {
            DisableAction(actionReference);
        }
    }
}
