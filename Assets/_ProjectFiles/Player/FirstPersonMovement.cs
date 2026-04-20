using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private FirstPersonParameters _parameters;
    private Speed _speed;
    private InputHandler _inputHandler;

    [Inject]
    public void Construct(FirstPersonParameters parameters, Speed speed, InputHandler inputHandler)
    {
        _parameters = parameters;
        _speed = speed;
        _inputHandler = inputHandler;
    }

    private void Start() => _rb = GetComponent<Rigidbody>();

    private void FixedUpdate() => Move();

    private void Move()
    {
        Vector2 movementAxes = _inputHandler.MovementInput();
        Vector3 direction = (transform.right * movementAxes.x + transform.forward * movementAxes.y).normalized;
        _rb.linearVelocity = direction * _speed.FinalSpeed;
    }
}