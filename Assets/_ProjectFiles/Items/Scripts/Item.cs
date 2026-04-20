using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField] public ItemParameters Parameters { get; private set; }
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void Move(Transform parent)
    {
        bool isPickUp = parent != null;
        _collider.isTrigger = isPickUp;
        transform.SetParent(parent);
    }

    public string GetHintText() => Parameters.Hint;

    void IInteractable.Interact()
    {
        PlayerItemsController.Instance.TryTakeItem(this);
    }
}