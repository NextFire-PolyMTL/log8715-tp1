using System.Collections.Generic;

public readonly struct Backups : IComponent
{
    public Backups(Queue<WorldBackup> worldBackups)
    {
        WorldBackups = worldBackups;
    }

    public readonly Queue<WorldBackup> WorldBackups;
}

public readonly struct WorldBackup : IComponent
{
    public WorldBackup(float timestamp, World world)
    {
        Timestamp = timestamp;
        World = world;
    }

    public readonly float Timestamp;
    public readonly World World;
}

public readonly struct LastBacktrack : IComponent
{
    public LastBacktrack(float timestamp)
    {
        Timestamp = timestamp;
    }

    public readonly float Timestamp;
}
