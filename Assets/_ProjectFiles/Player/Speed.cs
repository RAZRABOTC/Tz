using System.Collections.Generic;

public class Speed
{
    private List<SpeedModifier> _speedModifiers;
    private readonly float _initialSpeed;

    public Speed(FirstPersonParameters parameters)
    {
        _initialSpeed = parameters.InitialMovementSpeed;
        _speedModifiers = new();
    }

    public float FinalSpeed
    {
        get
        {
            float finalFactor = 1;
            foreach (var speedModifier in _speedModifiers) if(speedModifier.IsOn) finalFactor *= speedModifier.Factor;
            return _initialSpeed * finalFactor;
        }
    }
       
    public SpeedModifier AddModifier(float factor, bool isOn)
    {
        SpeedModifier modifier = new (factor, isOn);
        _speedModifiers.Add(modifier);
        return modifier;
    }
}

public class SpeedModifier
{
    public readonly float Factor;
    public bool IsOn;

    public SpeedModifier(float factor, bool isOn)
    {
        Factor = factor;
        IsOn = isOn;
    }
}
