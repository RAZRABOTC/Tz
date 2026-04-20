using UnityEngine;

public class AnimatorBoolPlayer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName;

    public void SetAnimPlay(bool isOn)
    {
        _animator.SetBool(_boolName, isOn);
    }

    public void SwitchAnimPlay()
    {
        SetAnimPlay(!_animator.GetBool(_boolName));
    }
}