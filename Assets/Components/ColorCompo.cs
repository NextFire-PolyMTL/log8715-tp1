using UnityEngine;

//Color of an entity
public readonly struct ColorCompo : IComponent
{
    public ColorCompo(Color shapeColor)
    {
        ShapeColor = shapeColor;
    }

    //Color of the entity
    public readonly Color ShapeColor;
}
