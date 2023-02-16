//Size of an entity
public readonly struct Size : IComponent
{
    public Size(int scale)
    {
        Scale = scale;
    }

    public readonly int Scale;
}
