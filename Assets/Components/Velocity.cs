//Velocity of an entity
public readonly struct Velocity : IComponent
{
    public Velocity(float vx, float vy)
    {
        Vx = vx;
        Vy = vy;
    }

    public readonly float Vx;
    public readonly float Vy;
}
