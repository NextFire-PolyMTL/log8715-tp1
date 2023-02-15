using UnityEngine;

public class PositionSys : IPhysicSystem
{
    public string Name => nameof(PositionSys);

    public void UpdateSystem()
    {
        var screenBoundary = World.Instance.GetSingleton<ScreenBoundary>().Value;

        //We first set the position and the velocity resulting from collisions
        Utils.PhysicsForEach<IsColliding>((entity, isColliding) =>
        {
            //No need to update the position or the velocity if there is no collision...
            if (!isColliding.HasValue)
            {
                return;
            }


            var position = World.Instance.GetComponent<Position>(entity);
            var velocity = World.Instance.GetComponent<Velocity>(entity);
            var scale = World.Instance.GetComponent<Size>(entity).Value.Scale;
            var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);

            /*If the entity is related to a isColliding tag but have not collidingWith component associated with it,
            it means that the corresponding object collided with the screen boundary*/
            if (isColliding.HasValue && !collidingWith.HasValue)

            {
                //If the object is outside the X an Y screen boundaries
                if (Mathf.Abs(position.Value.X) + (scale >> 1) >= screenBoundary.Value.x
                && Mathf.Abs(position.Value.Y) + (scale >> 1) >= screenBoundary.Value.y)
                {
                    /*To simulate the collision with the screen, we consider the result of a collision
                    bewteen the object and another shadow object with the same configuration but having an opposite speed*/
                    var newPosVit = CollisionUtility.CalculateCollision(
                                            new Vector2(position.Value.X, position.Value.Y),
                                            new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                                            scale,
                                            new Vector2(position.Value.X, position.Value.Y),
                                            new Vector2(-velocity.Value.Vx, -velocity.Value.Vy),
                                            scale
                                        );
                    World.Instance.SetComponent<Position>(entity, new Position(newPosVit.position1[0], newPosVit.position1[1]));
                    World.Instance.SetComponent<Velocity>(entity, new Velocity(newPosVit.velocity1[0], newPosVit.velocity1[1]));

                }

                //If the object is colliding with the X boundary
                else if (Mathf.Abs(position.Value.X) + (scale >> 1) >= screenBoundary.Value.x)
                {
                    /*To simulate the collision with the x screenboundary, we consider the result of a collision
                    bewteen the object and another shadow object with the same configuration but having an opposite speed
                    regarding the x-axis*/
                    var newPosVit = CollisionUtility.CalculateCollision(
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                        scale,
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(-velocity.Value.Vx, velocity.Value.Vy),
                        scale
                    );
                    World.Instance.SetComponent<Position>(entity, new Position(newPosVit.position1[0], newPosVit.position1[1]));
                    World.Instance.SetComponent<Velocity>(entity, new Velocity(newPosVit.velocity1[0], newPosVit.velocity1[1]));

                }

                //If the object is colliding the Y boundary
                else if (Mathf.Abs(position.Value.Y) + (scale >> 1) >= screenBoundary.Value.y)
                {
                    /*To simulate the collision with the y screenboundary, we consider the result of a collision
                    bewteen the object and another shadow object with the same configuration but having an opposite speed
                    regarding the y-axis*/
                    var newPosVit = CollisionUtility.CalculateCollision(
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                        scale,
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(velocity.Value.Vx, -velocity.Value.Vy),
                        scale
                    );
                    World.Instance.SetComponent<Position>(entity, new Position(newPosVit.position2[0], newPosVit.position2[1]));
                    World.Instance.SetComponent<Velocity>(entity, new Velocity(newPosVit.velocity2[0], newPosVit.velocity2[1]));
                }

                /*We delete the entity, after the execution of other systems, in another system (CommandBufferSys)
                which executes the singleton CommandBuffer*/
                Utils.AddCommandToBuffer(() =>
                {
                    World.Instance.RemoveComponent<IsColliding>(entity);
                });
            }

            /*If the object is colliding with other objects, we update its position, velocity and size
            with the result of the collisions*/
            if (isColliding.HasValue && collidingWith.HasValue)
            {

                var collidedShapes = collidingWith.Value.CollidedShapes;
                var collidedShapesPosition = collidingWith.Value.CollidedShapesPosition;
                var collidedShapesVelocity = collidingWith.Value.CollidedShapesVelocity;
                var collidedShapesSize = collidingWith.Value.CollidedShapesSize;

                for (int i = 0; i < collidedShapes.Count; i++)
                {

                    var position2 = collidedShapesPosition[i];
                    var velocity2 = collidedShapesVelocity[i];
                    var scale2 = collidedShapesSize[i].Scale;

                    //We simulate the collision between the two entities and get the result
                    var newPosVit = CollisionUtility.CalculateCollision(
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                        scale,
                        new Vector2(position2.X, position2.Y),
                        new Vector2(velocity2.Vx, velocity2.Vy),
                        scale2
                    );
                    World.Instance.SetComponent<Position>(entity, new Position(newPosVit.position1[0], newPosVit.position1[1]));
                    World.Instance.SetComponent<Velocity>(entity, new Velocity(newPosVit.velocity1[0], newPosVit.velocity1[1]));

                }

                /*We delete the entity, after the execution of other systems, in another system (CommandBufferSys)
                which executes the singleton CommandBuffer*/
                Utils.AddCommandToBuffer(() =>
                {
                    World.Instance.RemoveComponent<IsColliding>(entity);
                    World.Instance.RemoveComponent<CollidingWith>(entity);
                });

            }


        });

        //We update the position of all entities (except those which have a IsStatic tag among their components)
        Utils.PhysicsForEach<Velocity>((entity, velocity) =>
        {
            var isStatic = World.Instance.GetComponent<IsStatic>(entity);
            if (isStatic.HasValue)
            {
                return;
            }
            var position = World.Instance.GetComponent<Position>(entity);
            World.Instance.SetComponent<Position>(entity, new Position(position.Value.X + velocity.Value.Vx, position.Value.Y + velocity.Value.Vy));
        });
    }
}
