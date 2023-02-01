public class PositionSys : ISystem
{
    public string Name => "PositionSys";

    public void UpdateSystem()
    {
        World.Instance.ForEach<Velocity>((entity, velocity) =>
        {
            var position = World.Instance.GetComponent<Position>(entity);
            World.Instance.SetComponent<Position>(entity, new Position(position.X + velocity.Vx, position.Y + velocity.Vy));
        });
    }
}
