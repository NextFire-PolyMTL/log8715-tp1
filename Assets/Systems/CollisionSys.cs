using System.Collections.Generic;
using UnityEngine;

public class CollisionSys : IPhysicSystem
{
    public string Name => nameof(CollisionSys);

    public void UpdateSystem()
    {
        var screenBoundary = World.Instance.GetSingleton<ScreenBoundary>().Value;


        Utils.PhysicsForEach<Position>((entity, position) =>
        {
            var isStatic = World.Instance.GetComponent<IsStatic>(entity);
            if (isStatic.HasValue)
            {
                return;
            }

            var velocity = World.Instance.GetComponent<Velocity>(entity);
            var scale = World.Instance.GetComponent<Size>(entity).Value.Scale;

            if (Mathf.Abs(position.Value.X) + (scale >> 1) >= screenBoundary.Value.x)
            {
                if (Mathf.Sign(position.Value.X * velocity.Value.Vx) > 0)
                {
                    World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
                }
            }
            if (Mathf.Abs(position.Value.Y) + (scale >> 1) >= screenBoundary.Value.y)
            {
                if (Mathf.Sign(position.Value.Y * velocity.Value.Vy) > 0)
                {
                    World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
                }
            }

            /**
            if (Mathf.Abs(position.Value.X) + (scale >> 1) >= screenBoundary.Value.x || Mathf.Abs(position.Value.Y) + (scale >> 1) >= screenBoundary.Value.y)
            {

                World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
                Debug.Log("screen collision");

            }
            */
            //Code à discuter

            Utils.PhysicsForEach<Position>((entity2, position2) =>
            {
                var scale2 = World.Instance.GetComponent<Size>(entity2);
                var velocity2 = World.Instance.GetComponent<Velocity>(entity2);
                if (entity2.Id != entity.Id)
                {
                    if (Mathf.Sqrt(
                            Mathf.Pow((position.Value.X - position2.Value.X), 2)
                            + Mathf.Pow((position.Value.Y - position2.Value.Y), 2))
                        <= ((scale + scale2.Value.Scale) >> 1))
                    {
                        World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
                        var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);

                        if (!collidingWith.HasValue)
                        {
                            World.Instance.SetComponent<CollidingWith>(entity,
                                new CollidingWith(
                                    new List<uint> { entity2.Id },
                                    new List<Position> { position2.Value },
                                    new List<Velocity> { velocity2.Value },
                                    new List<Size> { scale2.Value }
                                )
                            );
                            //Debug.Log("coll");
                        }
                        else
                        {
                            //c moche à changer /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
                            var collidedShapes = collidingWith.Value.CollidedShapes;
                            collidedShapes.Add(entity2.Id);

                            var collidedShapesPosition = collidingWith.Value.CollidedShapesPosition;
                            collidedShapesPosition.Add(position2.Value);

                            var collidedShapesVelocity = collidingWith.Value.CollidedShapesVelocity;
                            collidedShapesVelocity.Add(velocity2.Value);

                            var collidedShapesSize = collidingWith.Value.CollidedShapesSize;
                            collidedShapesSize.Add(scale2.Value);
                        }
                    }
                }
            });
        });
    }
}
