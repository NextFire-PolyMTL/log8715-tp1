using System;
using System.Collections.Generic;

public interface IPhysicSystem : ISystem { }

public class Utils
{
    // Cloner for our components
    public static IComponent ComponentCloner(IComponent component)
    {
        switch (component)
        {
            // case everything that needs deep copies (reference types)
            case CollidingWith collidingWith:
                var collidedShapes = new List<uint>(collidingWith.CollidedShapes);
                var collidedShapesPosition = new List<Position>(collidingWith.CollidedShapesPosition);
                var collidedShapesVelocity = new List<Velocity>(collidingWith.CollidedShapesVelocity);
                var collidedShapesSize = new List<Size>(collidingWith.CollidedShapesSize);
                return new CollidingWith(collidedShapes, collidedShapesPosition, collidedShapesVelocity, collidedShapesSize);
            case CommandBuffer commandBuffer:
                var commands = new Queue<Action>(commandBuffer.Commands);
                return new CommandBuffer(commands);

            // in case of value types (or wanted shallow copies), return the same instance
            default:
                return component;
        }
    }

    // ForEach that skips ignored entities
    public static void PhysicsForEach<T>(Action<Entity, T?> action) where T : struct, IComponent
    {
        World.Instance.ForEach<T>((entity, component) =>
        {
            var isIgnored = World.Instance.GetComponent<PhysicsIgnore>(entity);
            if (!isIgnored.HasValue)
            {
                action(entity, component);
            }
        });
    }

    // Helper to add a command to the command buffer
    public static void AddCommandToBuffer(Action command)
    {
        var commandBuffer = World.Instance.GetSingleton<CommandBuffer>();
        commandBuffer.Value.Commands.Enqueue(command);
    }
}
