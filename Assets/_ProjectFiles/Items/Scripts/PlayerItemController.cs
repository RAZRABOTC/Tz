using UnityEngine;
using Zenject;

public class PlayerItemsController : MonoBehaviour
{
    public Item CurrentItem { get; private set; }
    public static PlayerItemsController Instance;
    private PlayerItemsHandler _itemsHanlder;
    private InputHandler _inputHandler;
    private ItemMenu _itemMenu;

    [Inject]
    public void Construct(FirstPersonParameters parameters, InputHandler inputHandler, PlayerItemsHandler itemsHandler, ItemMenu itemMenu)
    {
        _inputHandler = inputHandler;
        _itemsHanlder = itemsHandler;
        _itemMenu = itemMenu;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void TryTakeItem(Item item)
    {
        if (!IsEmpty()) return;
        CurrentItem = item;
        _itemsHanlder.Take(item);
        _itemMenu.OpenMenu(item.Parameters);
    }

    public void RemoveItem()
    {
        Destroy(CurrentItem.gameObject);
    }

    public void PutItem(Transform newParent)
    {
        _itemsHanlder.PutBack(CurrentItem, newParent);
        CurrentItem = null;
    }

    private bool IsEmpty() => CurrentItem == null;
}