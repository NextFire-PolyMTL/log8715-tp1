using System.Collections.Generic;
using UnityEngine;

public class InitSys : ISystem
{
    public string Name => "InitSys";

    // TODO: le bouger dans un singleton
    // (je crois que c'est interdit d'avoir des attributs dans les systèmes)
    private bool _initializing = true;

    public void UpdateSystem()
    {
        if (_initializing)
        {
            _initializing = false;

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
                ECSManager.Instance.CreateShape((uint)entity.Id, shape.initialSize);
            }

            World.Instance.SetSingleton<ScreenBoundary>(
                new ScreenBoundary(
                    Camera.main.ScreenToWorldPoint(
                        new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z))));

            // Setting the seed of the random Generator
            UnityEngine.Random.InitState(ECSManager.Instance.Config.seed);

            // Setup backups
            World.Instance.SetSingleton<Backups>(new Backups(new Queue<WorldBackup>()));
        }
    }
}
