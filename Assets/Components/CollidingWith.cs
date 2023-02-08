using System.Collections.Generic;

public readonly struct CollidingWith : IComponent
{
    public CollidingWith(List<int> collidedShapes, List<Position> collidedShapesPosition, List<Velocity> collidedShapesVelocity, List<Size> collidedShapesSize)
    {
        CollidedShapes = collidedShapes;
        CollidedShapesPosition = collidedShapesPosition;
        CollidedShapesVelocity = collidedShapesVelocity;
        CollidedShapesSize = collidedShapesSize;
    }

    public readonly List<int> CollidedShapes;
    public readonly List<Position> CollidedShapesPosition;
    public readonly List<Velocity> CollidedShapesVelocity;
    public readonly List<Size> CollidedShapesSize;
}
