using UnityEngine;

public class BacktrackSys : ISystem
{
    public string Name => nameof(BacktrackSys);

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
        var clone = World.Instance.Clone(Utils.ComponentCloner);
        worldBackups.Enqueue(new Backups.WorldBackup(currTime, clone));

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
                Debug.Log($"[Backtrack] Rollback of {currTime - backup.Timestamp} seconds");

                // Delete all current shapes
                World.Instance.ForEach<Position>((entity, position) =>
                {
                    ECSManager.Instance.DestroyShape((uint)entity.Id);
                });

                World.Instance = backup.World;

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
}
