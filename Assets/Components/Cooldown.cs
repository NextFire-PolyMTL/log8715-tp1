//Cooldown use for user's input and protection 
public readonly struct Cooldown : IComponent
{
    public Cooldown(float t)
    {
        Time = t;
    }

    public readonly float Time;
}
