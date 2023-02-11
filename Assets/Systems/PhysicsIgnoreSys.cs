public class PhysicsIgnoreSys : ISystem
{
    public string Name => nameof(PhysicsIgnoreSys);

    public void UpdateSystem()
    {
        World.Instance.ForEach<Position>((entity, position) =>
        {
            if (position.Value.X > 0)
            {
                World.Instance.SetComponent<PhysicsIgnore>(entity, new PhysicsIgnore());
            }
        });
    }
}
