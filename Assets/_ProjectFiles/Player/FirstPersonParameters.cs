using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerParameters", menuName = "PlayerParameters")]
public class FirstPersonParameters : ScriptableObject
{
    [Header("Movement")]
    [field: SerializeField] public float InitialMovementSpeed { get; private set; }
    [Header("Crouch")]
    [field: SerializeField] public float ChangeMovementVelocityCrouchFactor { get; private set; }
    [field: SerializeField] public float CrouchedHight { get; private set; }
    [field: SerializeField] public float InitialHight { get; private set; }

    [Header("Jump")]
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public float JumpDuration { get; private set; }
    [field: SerializeField] public float GroundCheckRadius { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public AnimationCurve JumpCurve { get; private set; }

    [Header("Camera")]
    [field: SerializeField] public float Sensivity { get; private set; }
    [field: SerializeField] public Vector2 RestrictionX { get; private set; }
}