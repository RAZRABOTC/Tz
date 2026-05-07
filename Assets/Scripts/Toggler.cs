using UnityEngine;

public class Toggler : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    public void Toggle(bool value)
    {
        _gameObject.SetActive(value);
    }

    public void Toggle()
    {
        Toggle(!_gameObject.activeSelf);
    }
}
