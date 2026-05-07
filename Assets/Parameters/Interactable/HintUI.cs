using TMPro;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hintText;
    [SerializeField] private GameObject _hintBackground;

    public void ShowHint(string hint)
    {
        ToggleHint(true);
        _hintText.text = hint;
    }

    public void ToggleHint(bool isOn) => _hintBackground.SetActive(isOn);
}
