using System.Collections.Generic;

public class Utils
{
    public static IComponent ComponentCloner(IComponent component)
    {
        switch (component)
        {
            // case everything that needs deep copy (reference types)
            case CollidingWith collidingWith:
                var CollidedShapes = new List<uint>(collidingWith.CollidedShapes);
                var CollidedShapesPosition = new List<Position>(collidingWith.CollidedShapesPosition);
                var CollidedShapesVelocity = new List<Velocity>(collidingWith.CollidedShapesVelocity);
                var CollidedShapesSize = new List<Size>(collidingWith.CollidedShapesSize);
                return new CollidingWith(CollidedShapes, CollidedShapesPosition, CollidedShapesVelocity, CollidedShapesSize);

            // in case of value types, just return the same instance,
            // it will automatically be copied
            default:
                return component;
        }
    }
}
