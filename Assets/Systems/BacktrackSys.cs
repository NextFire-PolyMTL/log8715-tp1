using UnityEngine;

//Take care of the backtracking in the simulation
public class BacktrackSys : ISystem
{
    public string Name => nameof(BacktrackSys);

    // How long we backtrack
    const float BACKTRACK_TIME = 3;
    // Cooldown between backtracks
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

        // Rollback (or cooldown status) on space press
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
                    ECSManager.Instance.DestroyShape(entity.Id);
                });

                // Swap the world instance with the backup
                World.Instance = backup.World;

                // Recreate shapes with the matching ids from the backup
                World.Instance.ForEach<Position>((entity, position) =>
                {
                    // We don't care about their size (nor their position or color)
                    // RenderSys will take care of that in the next frame
                    ECSManager.Instance.CreateShape(entity.Id, 0);
                });

                // Update the last backtrack time
                World.Instance.SetSingleton<LastBacktrack>(new LastBacktrack(currTime));
            }
        }
    }
}
