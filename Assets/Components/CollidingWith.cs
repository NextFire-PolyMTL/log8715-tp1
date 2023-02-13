using System.Collections.Generic;

public readonly struct CollidingWith : IComponent
{
    /*
    The follow component contains data about shapes which collide with the entity related to the component
    */
    public CollidingWith(
        List<uint> collidedShapes,
        List<Position> collidedShapesPosition,
        List<Velocity> collidedShapesVelocity,
        List<Size> collidedShapesSize)
    {
        CollidedShapes = collidedShapes;
        CollidedShapesPosition = collidedShapesPosition;
        CollidedShapesVelocity = collidedShapesVelocity;
        CollidedShapesSize = collidedShapesSize;
    }

    //List of id of the objects which collide with the entity
    public readonly List<uint> CollidedShapes;

    //List of position of the objects which collide with the entity
    public readonly List<Position> CollidedShapesPosition;

    //List of velocity of the objects which collide with the entity
    public readonly List<Velocity> CollidedShapesVelocity;

    //List of size of the objects which collide the with entity
    public readonly List<Size> CollidedShapesSize;
}
