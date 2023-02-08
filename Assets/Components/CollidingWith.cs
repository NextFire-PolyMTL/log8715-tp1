public readonly struct CollidingWith : IComponent
{
    public CollidingWith(int[] collidedShapes,Position[] collidedShapesPosition,Velocity[] collidedShapesVelocity,Size[] collidedShapesSize)
    {
        CollidedShapes = collidedShapes;
        CollidedShapesPosition=collidedShapesPosition;
        CollidedShapesVelocity=collidedShapesVelocity;
        CollidedShapesSize=collidedShapesSize;
    }

    public readonly int[] CollidedShapes;
    public readonly Position[] CollidedShapesPosition;
    public readonly Velocity[] CollidedShapesVelocity;
    public readonly Size[] CollidedShapesSize;
}
