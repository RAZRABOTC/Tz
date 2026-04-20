using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ItemParameters : InteractableParameters
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField, TextArea] public string Description { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
}