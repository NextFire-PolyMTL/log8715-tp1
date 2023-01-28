public class InitSys : ISystem
{
    public string Name => "InitSys";

    private bool initializing = true;

    public void UpdateSystem()
    {
        if (initializing)
        {
            initializing = false;
            foreach (var shape in ECSManager.Instance.Config.circleInstancesToSpawn)
            {
                var entity = World.Instance.CreateEntity();
                World.Instance.AddComponent<Size>(entity, new Size(shape.initialSize));
                World.Instance.AddComponent<Position>(entity, new Position(shape.initialPosition.x, shape.initialPosition.y));
                World.Instance.AddComponent<Velocity>(entity, new Velocity(shape.initialVelocity.x, shape.initialVelocity.y));
                ECSManager.Instance.CreateShape((uint)entity.Id, shape.initialSize);
            }
        }
    }
}
