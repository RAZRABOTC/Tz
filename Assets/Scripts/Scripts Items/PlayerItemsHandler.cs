using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemsHandler : MonoBehaviour
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private float _takeDuration;
    [SerializeField] private float _throwForce;
    [SerializeField] private Camera _camera;

    public void Take(Item item)
    {
        item.GetRb().isKinematic = true;
        item.Move(_handPoint);
        MoveItem(item);
    }

    public void PutBack(Item item, Transform newParent)
    {
        if (item == null) return;
        item.Move(newParent);
        MoveItem(item);
    }

    public void Throw(Item item)
    {
        item.Move(null);
        item.GetRb().isKinematic = false;
        //item.GetRb().AddForce(_throwForce * _camera.transform.forward, ForceMode.Impulse);
    }

    private void MoveItem(Item item)
    {
        item.transform.DOLocalMove(Vector3.zero, _takeDuration).SetEase(Ease.OutQuint);
        item.transform.DOLocalRotate(Vector3.zero, _takeDuration).SetEase(Ease.OutQuint);
    }
}