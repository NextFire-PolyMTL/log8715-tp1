using System.Collections.Generic;
using UnityEngine;

public class BacktrackSys : ISystem
{
    public string Name => "BacktrackSys";

    const float BACKTRACK_TIME = 3;
    const float BACKTRACK_COOLDOWN = 3;

    public void UpdateSystem()
    {
        var backups = World.Instance.GetSingleton<Backups>();
        var worldBackups = backups.Value.WorldBackups;

        var currTime = Time.realtimeSinceStartup;

        // Cleanup old backups
        while (worldBackups.Count > 0 && currTime - worldBackups.Peek().Timestamp > BACKTRACK_TIME)
        {
            worldBackups.Dequeue();
        }

        // Add new backup
        var clone = World.Instance.Clone(ComponentCloner);
        worldBackups.Enqueue(new WorldBackup(currTime, clone));

        // Rollback on space
        if (Input.GetKey(KeyCode.Space))
        {
            var lastBacktrack = World.Instance.GetSingleton<LastBacktrack>();
            if (lastBacktrack.HasValue && (currTime - lastBacktrack.Value.Timestamp) < BACKTRACK_COOLDOWN)
            {
                var remaining = BACKTRACK_COOLDOWN - (currTime - lastBacktrack.Value.Timestamp);
                Debug.Log($"[Backtrack] Cooldown: {remaining} seconds");
            }
            else
            {
                var backup = worldBackups.Dequeue();
                World.Instance = backup.World;
                Debug.Log($"[Backtrack] Rollback of {currTime - backup.Timestamp} seconds");

                // Delete all current shapes
                World.Instance.ForEach<Position>((entity, position) =>
                {
                    ECSManager.Instance.DestroyShape((uint)entity.Id);
                });

                // Recreate shapes with the matching ids from the backup
                World.Instance.ForEach<Position>((entity, position) =>
                {
                    // We don't care about their size (nor their position or color)
                    // RenderSys will take care of that in the next frame
                    ECSManager.Instance.CreateShape((uint)entity.Id, 0);
                });

                World.Instance.SetSingleton<LastBacktrack>(new LastBacktrack(currTime));
            }
        }
    }

    private IComponent ComponentCloner(IComponent component)
    {
        switch (component)
        {
            // case everything that needs deep copy (reference types)
            case CollidingWith collidingWith:
                var CollidedShapes = new List<int>(collidingWith.CollidedShapes);
                var CollidedShapesPosition = new List<Position>(collidingWith.CollidedShapesPosition);
                var CollidedShapesVelocity = new List<Velocity>(collidingWith.CollidedShapesVelocity);
                var CollidedShapesSize = new List<Size>(collidingWith.CollidedShapesSize);
                return new CollidingWith(CollidedShapes, CollidedShapesPosition, CollidedShapesVelocity, CollidedShapesSize);

            // default to shallow copy (value types)
            default:
                return component;
        }
    }
}
