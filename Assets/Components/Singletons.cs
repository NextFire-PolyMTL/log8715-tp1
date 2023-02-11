using System;
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
    public readonly struct WorldBackup
    {
        public WorldBackup(float timestamp, World world)
        {
            Timestamp = timestamp;
            World = world;
        }

        public readonly float Timestamp;
        public readonly World World;
    }

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

public readonly struct CommandBuffer : IComponent
{
    public CommandBuffer(Queue<Action> commands)
    {
        Commands = commands;
    }

    public readonly Queue<Action> Commands;
}
