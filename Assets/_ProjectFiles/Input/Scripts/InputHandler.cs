using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _crouchAction;
    [SerializeField] private InputActionReference _interactionAction;

    private const string _move = "Move";
    private const string _rotationItemDelta = "RotationItemDelta";

    public Action OnJump;
    public Action OnCrouch;
    public Action OnInteraction;

    public bool IsInteractionPressed => _interactionAction.action.WasPressedThisFrame();
    public bool IsInteractionHeld => _interactionAction.action.IsPressed();
    public bool IsInteractionReleased => _interactionAction.action.WasReleasedThisFrame();

    public Vector2 RotationItemDelta() => _playerInput.actions[_rotationItemDelta].ReadValue<Vector2>();
    public Vector2 MovementInput() => _playerInput.actions[_move].ReadValue<Vector2>();
    public Vector2 CameraMouseInput() => Mouse.current.delta.value;

    private void OnEnable()
    {
        _jumpAction.action.performed += ctx => OnJump?.Invoke();
        _crouchAction.action.performed += ctx => OnCrouch?.Invoke();
        _interactionAction.action.performed += ctx => OnInteraction?.Invoke();
    }

    private void OnDisable()
    {
        _jumpAction.action.performed -= ctx => OnJump?.Invoke();
        _crouchAction.action.performed -= ctx => OnCrouch?.Invoke();
        _interactionAction.action.performed -= ctx => OnInteraction?.Invoke();
    }
}