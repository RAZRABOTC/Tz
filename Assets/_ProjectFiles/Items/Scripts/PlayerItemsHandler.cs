using DG.Tweening;
using UnityEngine;

public class PlayerItemsHandler : MonoBehaviour
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private float _takeDuration;
 
    public void Take(Item interactable)
    {
        interactable.Move(_handPoint);
        MoveItem(interactable);
    }

    public void PutBack(Item interactable, Transform newParent)
    {
        if (interactable == null) return;
        interactable.Move(newParent);
        MoveItem(interactable);
    }

    private void MoveItem(Item interactable)
    {
        interactable.transform.DOLocalMove(Vector3.zero, _takeDuration).SetEase(Ease.OutQuint);
        interactable.transform.DOLocalRotate(Vector3.zero, _takeDuration).SetEase(Ease.OutQuint);
    }
}