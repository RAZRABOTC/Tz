using DG.Tweening;
using UnityEngine;
using Zenject;

public class ValveInteractable : InteractableWithConditions
{
    [SerializeField] private Transform _valveRotatingPart;
    [SerializeField] private Vector3 _rotationAxis = Vector3.up;
    [SerializeField] private float _maxRotationAngle = 360f;
    [SerializeField] private Transform _door;
    [SerializeField] private Quaternion _doorOpenRotation;
    [SerializeField] private Quaternion _doorClosedRotation;
    [SerializeField] private float _rotationSpeed = 90f;
    [SerializeField] private float _returnDuration = 0.5f;
    [SerializeField] private AnimationCurve _returnCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private bool _isBeingHeld;
    private float _currentRotationAngle;
    private Tween _currentRotationTween;
    private Tween _currentDoorTween;
    private InputHandler _inputHandler;

    [Inject]
    public void Construct(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    private void Update()
    {
        if (!_isBeingHeld) return;
        float deltaAngle = _rotationSpeed * Time.deltaTime;
        float newAngle = _currentRotationAngle + deltaAngle;

        if (newAngle >= _maxRotationAngle)
        {
            newAngle = _maxRotationAngle;
            StopHolding();
        }
        SetRotation(newAngle);
    }

    private void SetRotation(float angle)
    {
        _currentRotationAngle = Mathf.Clamp(angle, 0, _maxRotationAngle);
        _valveRotatingPart.localRotation = Quaternion.AngleAxis(_currentRotationAngle, _rotationAxis);
        float t = _currentRotationAngle / _maxRotationAngle;
        Quaternion doorTargetRotation = Quaternion.Lerp(_doorClosedRotation, _doorOpenRotation, t);
        _door.localRotation = doorTargetRotation;
    }

    public void StartHolding()
    {
        if (_isBeingHeld) return;
        _currentRotationTween?.Kill();
        _currentDoorTween?.Kill();
        _isBeingHeld = true;
    }

    public void StopHolding()
    {
        if (!_isBeingHeld) return;

        _isBeingHeld = false;
        float startAngle = _currentRotationAngle;
        Vector3 startDoorPosition = _door.localPosition;
        _currentRotationTween = DOTween.To(
            () => startAngle,
            x => SetRotation(x),
            0f,
            _returnDuration * (_currentRotationAngle / _maxRotationAngle)
        ).SetEase(_returnCurve);
        _currentRotationTween.OnComplete(() =>
        {
            _currentRotationAngle = 0;
            _valveRotatingPart.localRotation = Quaternion.identity;
            _door.localRotation = _doorClosedRotation;
        });
    }

    public override void InteractWithCheckedConditions()
    {
        //нарушил один из принципов солид
    }

    private void OnDestroy()
    {
        _currentRotationTween?.Kill();
        _currentDoorTween?.Kill();
    }
}