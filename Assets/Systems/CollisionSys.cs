using System.Collections.Generic;
using UnityEngine;

public class CollisionSys : ISystem
{
    public string Name => "CollisionSys";

    public void UpdateSystem()
    {
        World.Instance.ForEach<Position>((entity, position) =>
        {
            var radius = World.Instance.GetComponent<Size>(entity).Value.Radius;

            //Trouver mieux /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
            //(et en faire peut-être une cst....)
            var screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            if (Mathf.Abs(position.Value.X) + radius / 2 >= screenBoundary.x || Mathf.Abs(position.Value.Y) + radius / 2 >= screenBoundary.y)
            {
                World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
            }
            //Code à discuter

            World.Instance.ForEach<Position>((entity2, position2) =>
            {
                var radius2 = World.Instance.GetComponent<Size>(entity2);
                var velocity2 = World.Instance.GetComponent<Velocity>(entity2);
                if (entity2.Id != entity.Id)
                {
                    if (Mathf.Sqrt(Mathf.Pow((position.Value.X - position2.Value.X), 2)
                    + Mathf.Pow((position.Value.Y - position2.Value.Y), 2)) <= (radius + radius2.Value.Radius) / 2)
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
                                    new List<Size> { radius2.Value }
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
                            collidedShapesSize.Add(radius2.Value);
                        }
                    }
                }
            });


        });

    }
}
