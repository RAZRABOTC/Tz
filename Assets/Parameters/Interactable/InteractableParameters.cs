using UnityEngine;

[CreateAssetMenu(fileName = "Interactable", menuName = "Interactable")]
public class InteractableParameters : ScriptableObject
{
    [field: SerializeField] public string Hint { get; private set; }
}