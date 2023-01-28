public readonly struct CollidingWith : IComponent
{
    public CollidingWith(int[] collidedShapes)
    {
        CollidedShapes = collidedShapes;
    }

    public readonly int[] CollidedShapes;
}
