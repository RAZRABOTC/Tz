using UnityEngine;

public class ItemsSocket : MonoBehaviour, IInteractable
{
    [SerializeField] private string _hintText = "Положить";

    public void Interact()
    {
        PlayerItemsController.Instance.PutItem(transform);
    }

    public string GetHintText() => _hintText;
}