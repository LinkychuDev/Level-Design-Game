using UnityEngine;

public enum SubstanceType
{
    Normal,
    Frozen,
    Burning,
    Smoke,
    Wet
}
public interface ISubstance
{
    public SubstanceType SubstanceType { get; }
    
    public void Freeze();

    public void Melt();

    public void Steam();
    public void Ignite();
}
