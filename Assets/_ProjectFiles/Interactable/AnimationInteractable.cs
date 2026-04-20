using UnityEngine;

public class AnimationInteractable : InteractableWithConditions
{
    [SerializeField] private AnimatorBoolPlayer _animator;

    public override void InteractWithCheckedConditions()
    {
        _animator.SwitchAnimPlay();
    }
}