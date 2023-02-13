using UnityEngine;

public readonly struct ColorCompo : IComponent
{
    public ColorCompo(Color shapeColor)
    {
        ShapeColor = shapeColor;
    }

    public readonly Color ShapeColor;
}
