//Remove all PhysicsIgnore tag
public class PhysicsIgnoreResetSys : ISystem
{
    public string Name => nameof(PhysicsIgnoreResetSys);

    public void UpdateSystem()
    {
        World.Instance.ForEach<PhysicsIgnore>((entity, val) =>
        {
            World.Instance.RemoveComponent<PhysicsIgnore>(entity);
        });
    }
}
