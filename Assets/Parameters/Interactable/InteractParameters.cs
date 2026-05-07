using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class InteractParameters : MonoBehaviour
{
    [field: SerializeField] public float MaxDistance { get; private set; }
}
