using System.Collections.Generic;
using UnityEngine;

public class CollisionSys : ISystem
{
    public string Name => "CollisionSys";

    public void UpdateSystem()
    {
         var screenBoundary = World.Instance.GetSingleton<ScreenBoundary>().Value;

        World.Instance.ForEach<Position>((entity, position) =>
        {
            var scale = World.Instance.GetComponent<Size>(entity).Value.Scale;

            if (Mathf.Abs(position.Value.X) + scale / 2 >= screenBoundary.Value.x || Mathf.Abs(position.Value.Y) + scale / 2 >= screenBoundary.Value.y)
            {
                World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
            }
            //Code à discuter

            World.Instance.ForEach<Position>((entity2, position2) =>
            {
                var scale2 = World.Instance.GetComponent<Size>(entity2);
                var velocity2 = World.Instance.GetComponent<Velocity>(entity2);
                if (entity2.Id != entity.Id)
                {
                    if (Mathf.Sqrt(Mathf.Pow((position.Value.X - position2.Value.X), 2)
                    + Mathf.Pow((position.Value.Y - position2.Value.Y), 2)) <= (scale + scale2.Value.Scale) / 2)
                    {
                        World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
                        var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);

                        if (!collidingWith.HasValue)
                        {
                            World.Instance.SetComponent<CollidingWith>(entity,
                                new CollidingWith(
                                    new List<int> { entity2.Id },
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
