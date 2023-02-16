using System;
using System.Collections.Generic;
using UnityEngine;

// Set when InitSys is done
public readonly struct Initialized : IComponent { }

public readonly struct ScreenBoundary : IComponent
{
    public ScreenBoundary(Vector3 value)
    {
        this.Value = value;
    }

    public readonly Vector3 Value;
}

//Cooldown between mouse's input
public readonly struct ClickCooldown : IComponent
{
    public ClickCooldown(float value)
    {
        this.Time = value;
    }

    public readonly float Time;
}

// Backups of the world for backtracking
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

    // Only contains the last 3 seconds
    public readonly Queue<WorldBackup> WorldBackups;
}

// Last time the player backtracked (for cooldown)
public readonly struct LastBacktrack : IComponent
{
    public LastBacktrack(float timestamp)
    {
        Timestamp = timestamp;
    }

    public readonly float Timestamp;
}

// Command buffer queue
public readonly struct CommandBuffer : IComponent
{
    public CommandBuffer(Queue<Action> commands)
    {
        Commands = commands;
    }

    public readonly Queue<Action> Commands;
}
