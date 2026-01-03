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
    
    
    public Material[] _materials { get; }
    public void Freeze();

    public void Hover();
    
    public void UnHover();

    public void Melt();

    public void Steam();
    public void Ignite();
}
