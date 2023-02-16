//Set PhysicsIgnore tag for all antities at the right of the screen
public class PhysicsIgnoreSys : ISystem
{
    public string Name => nameof(PhysicsIgnoreSys);

    public void UpdateSystem()
    {
        World.Instance.ForEach<Position>((entity, position) =>
        {
            // Ignore physics for shapes that are on the right side of the screen
            if (position.Value.X > 0)
            {
                World.Instance.SetComponent<PhysicsIgnore>(entity, new PhysicsIgnore());
            }
        });
    }
}
