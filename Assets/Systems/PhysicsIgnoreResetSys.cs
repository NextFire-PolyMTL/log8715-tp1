public class PhysicsIgnoreResetSys : ISystem
{
    public string Name => nameof(PhysicsIgnoreResetSys);

    public void UpdateSystem()
    {
        World.Instance.ForEach<PhysicsIgnore>((entity, val) =>
        {
            if (val.HasValue)
            {
                World.Instance.RemoveComponent<PhysicsIgnore>(entity);
            }
        });
    }
}
