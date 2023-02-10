using System.Collections.Generic;
using UnityEngine;

public readonly struct Initialized : IComponent { }

public readonly struct ScreenBoundary : IComponent
{
    public ScreenBoundary(Vector3 value)
    {
        this.Value = value;
    }

    public readonly Vector3 Value;
}


public readonly struct Backups : IComponent
{
    public Backups(Queue<WorldBackup> worldBackups)
    {
        WorldBackups = worldBackups;
    }

    public readonly Queue<WorldBackup> WorldBackups;
}


public readonly struct LastBacktrack : IComponent
{
    public LastBacktrack(float timestamp)
    {
        Timestamp = timestamp;
    }

    public readonly float Timestamp;
}
