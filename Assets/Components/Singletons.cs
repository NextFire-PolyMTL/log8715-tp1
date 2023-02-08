using UnityEngine;

public readonly struct ScreenBoundary : IComponent
{
    public ScreenBoundary(Vector3 value)
    {
        this.Value = value;
    }

    public readonly Vector3 Value;
}
