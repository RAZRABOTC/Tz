using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField] public ItemParameters Parameters { get; private set; }
    private Collider _collider;
    private Rigidbody _rb;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Transform parent)
    {
        bool isPickUp = parent != null;
        _collider.isTrigger = isPickUp;
        transform.SetParent(parent);
    }

    public Rigidbody GetRb()
    {
        return _rb;
    }

    public string GetHintText() => Parameters.Hint;

    void IInteractable.Interact()
    {
        PlayerItemsController.Instance.TryTakeItem(this);
    }
}