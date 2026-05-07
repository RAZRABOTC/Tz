using TMPro;
using UnityEngine;
using Zenject;

public class ItemMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    private RotatableItemsController _rotatableItemsController;
    private bool _isOpened;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    private InputHandler _inputHandler; 
    private bool _isProcessingInput = true;

    [Inject]
    public void Construct(RotatableItemsController rotatableItemsController, InputHandler inputHander )
    {
        _rotatableItemsController = rotatableItemsController;
        _inputHandler = inputHander; 
    }

    private void Update()
    {
        if (_inputHandler.IsInteractionPressed)
        {
            CloseMenu();
        }
    }

    public void OpenMenu(ItemParameters parameters)
    {
        ToggleMenuRoutine(true);
        _rotatableItemsController.SetUpScene(parameters.Prefab);
        _descriptionText.text = parameters.Description;
        _isProcessingInput = true;
        StartCoroutine(UnlockInputNextFrame());
    }

    public void CloseMenu()
    {
        if (_isProcessingInput) return;
        if (!_isOpened) return;
        _rotatableItemsController.CleanUpScene();
        ToggleMenuRoutine(false);
    }
    
    private System.Collections.IEnumerator UnlockInputNextFrame()
    {
        yield return null;
        _isProcessingInput = false;
    }

    private void ToggleMenuRoutine(bool isOn)
    {
        _isOpened = isOn; 
        Cursor.lockState = isOn? CursorLockMode.None : CursorLockMode.Locked;
        _menu.SetActive(isOn);
    }
}