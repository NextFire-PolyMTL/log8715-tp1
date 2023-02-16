//Position of an entity
public readonly struct Position : IComponent
{
    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }

    public readonly float X;
    public readonly float Y;
}
