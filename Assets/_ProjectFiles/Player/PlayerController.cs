using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private FirstPersonMovement _firstPersonMovement;
    [SerializeField] private FirstPersonJump _firstPersonJump;
    [SerializeField] private FirstPersonCrouch _firstPersonCrouch;
    [SerializeField] private FirstPersonCamera _firstPersonCamera;
    [SerializeField] private InteractMachine _interactMachine;

    public void Toggle(bool isOn)
    {
        _firstPersonCamera.enabled = isOn;
        _firstPersonCrouch.enabled = isOn;
        _firstPersonJump.enabled = isOn;
        _firstPersonMovement.enabled = isOn;
        _interactMachine.enabled = isOn;
        PlayerItemsController.Instance.enabled = isOn;
    }
}