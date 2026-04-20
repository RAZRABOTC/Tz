using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class RotatableItemsController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject _scene;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _rotationVelocity;
    private Vector2 _rotationalSmoothedVelocity;
    private Transform _currentRotatableItem;
    private bool _isDragging;
    private InputHandler _inputHandler;

    [Inject]
    public void Construct(FirstPersonParameters parameters, InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    private void Update()
    {
        if (_isDragging) RotateItem();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }

    public void SetUpScene(GameObject rotatableItem)
    { 
        _scene.SetActive(true);
        _currentRotatableItem = Instantiate(rotatableItem, _spawnPoint.position, Quaternion.identity).transform;
    }

    public void CleanUpScene()
    { 
        _scene.SetActive(false);
        Destroy(_currentRotatableItem.gameObject);
    }

    private void RotateItem()
    { 
        Vector2 mouseDelta = _inputHandler.RotationItemDelta();
        _rotationalSmoothedVelocity = mouseDelta.normalized * _rotationVelocity;
        _currentRotatableItem.Rotate(Vector3.up, -_rotationalSmoothedVelocity.x, Space.World);
        _currentRotatableItem.Rotate(Vector3.right, _rotationalSmoothedVelocity.y, Space.World); 
    }
}
