using UnityEngine;
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
                World.Instance.SetComponent<Size>(entity, new Size(shape.initialSize));
                World.Instance.SetComponent<Position>(entity, new Position(shape.initialPosition.x, shape.initialPosition.y));
                World.Instance.SetComponent<Velocity>(entity, new Velocity(shape.initialVelocity.x, shape.initialVelocity.y));
                ECSManager.Instance.CreateShape((uint)entity.Id, shape.initialSize);
            }
            World.Instance.SetSingleton<ScreenBoundary>(
                new ScreenBoundary(
                    Camera.main.ScreenToWorldPoint(
                        new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z))));
        }
    }
}
