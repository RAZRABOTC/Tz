using UnityEngine;
using Zenject;

[RequireComponent(typeof(Camera))]
public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerBody;
    private Vector2 _localRotation;
    private FirstPersonParameters _parameters;
    private InputHandler _inputHandler;

    [Inject]
    public void Construct(FirstPersonParameters parameters, InputHandler inputHandler)
    {
        _parameters = parameters;
        _inputHandler = inputHandler;
    }

    private void Start() => Cursor.lockState = CursorLockMode.Locked;

    private void LateUpdate() => HandleCamera();

    private void HandleCamera()
    {
        Vector2 movementAxes = _inputHandler.CameraMouseInput() * _parameters.Sensivity;

        _localRotation.x = Mathf.Clamp(_localRotation.x + -movementAxes.y, _parameters.RestrictionX.x, _parameters.RestrictionX.y);
        _localRotation.y += movementAxes.x;

        transform.localRotation = Quaternion.Euler(_localRotation.x, 0, 0);
        _playerBody.localRotation = Quaternion.Euler(0, _localRotation.y, 0);
    }
}
