using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableWithConditions : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemParameters _neededItem;
    [SerializeField] private InteractableParameters _parameters;
    [SerializeField] private bool _onlyOnce;
    public UnityEvent OnInteract;
    private bool _isOpened;

    public string GetHintText() => _parameters.Hint;

    public void Interact()
    { 
        if(CheckConditions())
        { 
            if (_onlyOnce)
            {
                PlayerItemsController.Instance.RemoveItem();
                if (_isOpened) return;
            }
            _isOpened = true;
            InteractWithCheckedConditions();
            OnInteract?.Invoke();
        }
    }

    private bool CheckConditions()
    {
        return _neededItem == null ? true : (PlayerItemsController.Instance.CurrentItem != null ? PlayerItemsController.Instance.CurrentItem.Parameters == _neededItem : false);
    }

    public abstract void InteractWithCheckedConditions();
}