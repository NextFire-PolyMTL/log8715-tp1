using UnityEngine;

public class PositionSys : IPhysicSystem
{
    public string Name => nameof(PositionSys);

    public void UpdateSystem()
    {
        var screenBoundary = World.Instance.GetSingleton<ScreenBoundary>().Value;

        Utils.PhysicsForEach<IsColliding>((entity, isColliding) =>
        {
            /*
            if (!isColliding.HasValue)
            {
                return;
            }
            */

            var position = World.Instance.GetComponent<Position>(entity);
            var velocity = World.Instance.GetComponent<Velocity>(entity);
            var scale = World.Instance.GetComponent<Size>(entity).Value.Scale;
            var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);

            if (isColliding.HasValue && !collidingWith.HasValue)

            {
                //var shadowShape_Position=new Vector2(position.Value.X, position.Value.Y);
                //var shadowShape_Velocity=new Vector2(velocity.Value.Vx, velocity.Value.Vy);

                if (Mathf.Abs(position.Value.X) + (scale >> 1) >= screenBoundary.Value.x
                && Mathf.Abs(position.Value.Y) + (scale >> 1) >= screenBoundary.Value.y)
                {
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
                else if (Mathf.Abs(position.Value.X) + (scale >> 1) >= screenBoundary.Value.x)
                {
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

                else if (Mathf.Abs(position.Value.Y) + (scale >> 1) >= screenBoundary.Value.y)
                {
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

                World.Instance.RemoveComponent<IsColliding>(entity);
            }
            //code ci-dessous à discuter

            if (isColliding.HasValue && collidingWith.HasValue)
            {

                var collidedShapes = collidingWith.Value.CollidedShapes;
                var collidedShapesPosition = collidingWith.Value.CollidedShapesPosition;
                var collidedShapesVelocity = collidingWith.Value.CollidedShapesVelocity;
                var collidedShapesSize = collidingWith.Value.CollidedShapesSize;

                for (int i = 0; i < collidedShapes.Count; i++)
                {

                    var position2 = collidedShapesPosition[i]; //World.Instance.GetComponent<Position>(new Entity(collidedShape));
                    var velocity2 = collidedShapesVelocity[i];//World.Instance.GetComponent<Velocity>(new Entity(collidedShape));
                    //var scale2 = World.Instance.GetComponent<Size>(new Entity(collidedShapes[i])).Value.Scale;
                    var scale2 = collidedShapesSize[i].Scale;
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
                Utils.AddCommandToBuffer(() =>
                {
                    World.Instance.RemoveComponent<IsColliding>(entity);
                    World.Instance.RemoveComponent<CollidingWith>(entity);
                });
                //Debug.Log("erase");

            }


        });

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
