using System;
using UnityEngine;

public class ToggleInteractable : InteractableWithConditions
{
    [SerializeField] private Toggler[] _togglerForOn;
    [SerializeField] private Toggler[] _togglerForOff;

    public override void InteractWithCheckedConditions()
    {
        foreach(var toggle in _togglerForOn)
        {
            toggle.Toggle(true);
        }
        foreach (var toggle in _togglerForOff)
        {
            toggle.Toggle(false);
        }
    }
}
