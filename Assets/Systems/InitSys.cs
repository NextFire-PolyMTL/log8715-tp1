using System.Collections.Generic;
using UnityEngine;

public class InitSys : ISystem
{
    public string Name => nameof(InitSys);

    public void UpdateSystem()
    {
        var initialized = World.Instance.GetSingleton<Initialized>();

        if (!initialized.HasValue)
        {
            World.Instance.SetSingleton<Initialized>(new Initialized());

            foreach (var shape in ECSManager.Instance.Config.circleInstancesToSpawn)
            {
                var entity = World.Instance.CreateEntity();
                World.Instance.SetComponent<Size>(entity, new Size(shape.initialSize));
                World.Instance.SetComponent<Position>(entity, new Position(shape.initialPosition.x, shape.initialPosition.y));
                World.Instance.SetComponent<Velocity>(entity, new Velocity(shape.initialVelocity.x, shape.initialVelocity.y));
                if (shape.initialVelocity.x == 0 && shape.initialVelocity.y == 0)
                {
                    World.Instance.SetComponent<IsStatic>(entity, new IsStatic());
                }
                ECSManager.Instance.CreateShape(entity.Id, shape.initialSize);
            }

            World.Instance.SetSingleton<ScreenBoundary>(
                new ScreenBoundary(
                    Camera.main.ScreenToWorldPoint(
                        new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z))));

            // Setting the seed of the random Generator
            UnityEngine.Random.InitState(ECSManager.Instance.Config.seed);

            // Setup backups
            World.Instance.SetSingleton<Backups>(new Backups(new Queue<Backups.WorldBackup>()));
        }
    }
}
