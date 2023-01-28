public readonly struct Speed : IComponent
{
    public Speed(float vx, float vy)
    {
        Vx = vx;
        Vy = vy;
    }

    public readonly float Vx;
    public readonly float Vy;
}
