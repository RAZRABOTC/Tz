using UnityEngine;
using Zenject;

public class InteractMachine : MonoBehaviour
{
    [SerializeField] protected Transform _interactRayOrigin;
    [SerializeField] protected float _maxDistance;
    [SerializeField] private LayerMask _interactableLayerMask;
    private InputHandler _inputHandler;
    private IInteractable _lastInteractable;
    private ValveInteractable _currentValve;
    private HintUI _hintUI;
    private const string _interactKeyString = "E - ";
    private bool _isHoldingValve;

    [Inject]
    public void Construct(InputHandler inputHandler, HintUI hintUI)
    {
        _inputHandler = inputHandler;
        _hintUI = hintUI;
    }

    private void Update()
    {
        if (_isHoldingValve)
        {
            HandleValveHold();
            return;
        }
        CheckForInteractable();
    }

    private void HandleValveHold()
    {
        if (!_inputHandler.IsInteractionHeld)
        {
            _currentValve?.StopHolding();
            _isHoldingValve = false;
            _currentValve = null;
        }
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(_interactRayOrigin.position, _interactRayOrigin.forward);

        if (Physics.Raycast(ray, out var hit, _maxDistance, _interactableLayerMask))
        {
            if (hit.transform.TryGetComponent<IInteractable>(out var interactable))
            {
                if (interactable is ValveInteractable valve)
                {
                    _hintUI.ShowHint(_interactKeyString + " (Hold) - " + interactable.GetHintText());

                    if (_inputHandler.IsInteractionPressed && !_isHoldingValve)
                    {
                        _isHoldingValve = true;
                        _currentValve = valve;
                        valve.StartHolding();
                    }
                    if (interactable == _lastInteractable) return;
                    _lastInteractable = interactable;
                    return;
                }
                if (_inputHandler.IsInteractionPressed)
                {
                    interactable.Interact();
                }
                if (interactable == _lastInteractable) return;
                _lastInteractable = interactable;
                _hintUI.ShowHint(_interactKeyString + interactable.GetHintText());
                return;
            }
        }
        if (_lastInteractable != null)
        {
            _lastInteractable = null;
            _hintUI.ToggleHint(false);
        }
    }
}