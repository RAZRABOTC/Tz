using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonJump : MonoBehaviour
{
    [SerializeField] private bool _isGrounded;
    [SerializeField] private Transform _groundCheckPos;
    private Rigidbody _rb;
    private bool _isJumping;
    private FirstPersonParameters _parameters;
    private InputHandler _inputHandler;

    [Inject]
    public void Construct(FirstPersonParameters parameters, InputHandler inputHandler)
    {
        _parameters = parameters;
        _inputHandler = inputHandler;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _inputHandler.OnJump += Jump;
    }

    private void OnDisable()
    {
        _inputHandler.OnJump -= Jump;
    }

    private void Jump()
    {
        if (CheckIfIsGrounded() && !_isJumping)
        {
            _isJumping = true;
            JumpProcess();
        }
    }

    private async void JumpProcess()
    {
        float startTime = Time.fixedTime;
        float interpolatedTime = 0;

        while (interpolatedTime < 1)
        {
            interpolatedTime = (Time.fixedTime - startTime) / _parameters.JumpDuration;
            _rb.AddForce(Vector2.up * _parameters.JumpForce * _parameters.JumpCurve.Evaluate(interpolatedTime), ForceMode.VelocityChange);
            await Awaitable.FixedUpdateAsync();
        }
        _isJumping = false;
    }

    private bool CheckIfIsGrounded()
    {
        _isGrounded = Physics.CheckSphere(_groundCheckPos.position, _parameters.GroundCheckRadius, _parameters.GroundLayer);
        return _isGrounded;
    }
}