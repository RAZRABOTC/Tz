using UnityEngine;
using Zenject;

[RequireComponent(typeof(CapsuleCollider), typeof(FirstPersonMovement))]
public class FirstPersonCrouch : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    private CapsuleCollider _capsule;
    private SpeedModifier _crouchSpeedModifier;
    private bool _isCrouched;
    private Speed _speed;
    private FirstPersonParameters _parameters;
    private InputHandler _inputHandler;

    [Inject] 
    public void Construct(Speed speed, FirstPersonParameters parameters, InputHandler inputHandler)
    {
        _speed = speed;
        _parameters = parameters;
        _crouchSpeedModifier = _speed.AddModifier(parameters.ChangeMovementVelocityCrouchFactor, false);
        _inputHandler = inputHandler;
    }

    private void Start()
    {
        _capsule = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        _inputHandler.OnCrouch += Crouch;
    }

    private void OnDisable()
    {
        _inputHandler.OnCrouch -= Crouch;
    }

    private void Crouch()
    {
        if (CheckSpace()) return;
        _isCrouched = !_isCrouched;

        float difference = _parameters.InitialHight - _parameters.CrouchedHight;
        _crouchSpeedModifier.IsOn = _isCrouched;
        _camera.position += new Vector3(0, (_isCrouched ? -1 : 1) * difference, 0);
        _capsule.height = _isCrouched ? _parameters.CrouchedHight : _parameters.InitialHight;
        _capsule.center = new(_capsule.center.x, -(_isCrouched ? difference / 2 : 0), _capsule.center.z);
    }

    private bool CheckSpace()
    {
        return Physics.Raycast(transform.position, transform.up, _parameters.InitialHight / 2, _parameters.GroundLayer);
    }
}