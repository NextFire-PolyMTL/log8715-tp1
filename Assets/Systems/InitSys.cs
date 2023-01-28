using UnityEngine;

public class InitSys : ISystem
{
    public string Name => "InitSys";

    private uint currentId = 0;
    private bool initializing = true;

    public void UpdateSystem()
    {
        if (initializing)
        {
            initializing = false;

            foreach (var shape in ECSManager.Instance.Config.circleInstancesToSpawn)
            {
                var size = new Size(shape.initialSize);
                var position = new Position(shape.initialPosition.x, shape.initialPosition.y);
                var velocity = new Velocity(shape.initialVelocity.x, shape.initialVelocity.y);

                ComponentManager.Instance.AddComponent<Size>(size);
                ComponentManager.Instance.AddComponent<Position>(position);
                ComponentManager.Instance.AddComponent<Velocity>(velocity);

                // TODO: archetype?
                ECSManager.Instance.CreateShape(currentId++, shape.initialSize);
            }
        }

        Debug.Log(ComponentManager.Instance.DebugStr());
    }
}
