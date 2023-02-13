using System.Collections.Generic;
using UnityEngine;

public class CollisionSys : IPhysicSystem
{
    public string Name => nameof(CollisionSys);

    public void UpdateSystem()
    {
        //We get the screen boundaries
        var screenBoundary = World.Instance.GetSingleton<ScreenBoundary>().Value;


        Utils.PhysicsForEach<Position>((entity, position) =>
        {
            /*If the object is static, there is no need for collision detection on it
            (But of course, for the other non-static object that collid a static object
            we take into account the collision)*/
            var isStatic = World.Instance.GetComponent<IsStatic>(entity);
            if (isStatic.HasValue)
            {
                return;
            }

            //We get the velocity of the object
            var velocity = World.Instance.GetComponent<Velocity>(entity);
            //We get the scale of the object
            var scale = World.Instance.GetComponent<Size>(entity).Value.Scale;

            //If the object is outside the screen, we set a IsColliding tag:
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

            Utils.PhysicsForEach<Position>((entity2, position2) =>
            {
                var scale2 = World.Instance.GetComponent<Size>(entity2);
                var velocity2 = World.Instance.GetComponent<Velocity>(entity2);

                if (entity2.Id != entity.Id)
                {
                    //If entity2 is colliding with entity
                    if (Mathf.Sqrt(
                            Mathf.Pow((position.Value.X - position2.Value.X), 2)
                            + Mathf.Pow((position.Value.Y - position2.Value.Y), 2))
                        < (((scale + scale2.Value.Scale) >> 1)))
                    {
                        /*We save the information related to the shape which collids with entity into
                        */

                        var newPosVit = CollisionUtility.CalculateCollision(
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                        scale,
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(-velocity.Value.Vx, velocity.Value.Vy),
                        scale2.Value.Scale
                        );

                        /*We save the information related to the shape which collides with entity into
                        a CollidingWith component (and we set a tag IsColliding too)
                        */
                        World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
                        var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);

                        /*If the list of the CollidingWith component have not been initialized before,
                        we initialize it with the configuration of the colliding object*/
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
                        }
                        else
                        {
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
