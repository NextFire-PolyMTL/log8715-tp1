using UnityEngine;
using System;

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
            /*
            World.Instance.ForEach<Position>((entity2, position2) =>
            {
                var radius2 = World.Instance.GetComponent<Size>(entity2).Value.Radius;
                if (entity2.Id != entity.Id)
                {
                    if (Mathf.Sqrt(Mathf.Pow((position.Value.X - position2.Value.X),2)
                    + Mathf.Pow((position.Value.Y - position2.Value.Y),2)) <= (radius + radius2) / 2)
                    {
                        World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
                        var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);
                        if (!collidingWith.HasValue)
                        {
                            World.Instance.SetComponent<CollidingWith>(entity, new CollidingWith(new int[] {entity2.Id}));
                        }
                        else{
                            //c moche à changer /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
                            int[] collidedShapes = collidingWith.Value.CollidedShapes;
                            int[] newCollidedShapes = new int[collidedShapes.Length+1];
                            Array.Copy(collidedShapes, newCollidedShapes, collidedShapes.Length);
                            newCollidedShapes[collidedShapes.Length]=entity2.Id;
                            World.Instance.SetComponent<CollidingWith>(entity, new CollidingWith(newCollidedShapes));
                        }
                    }
                }
            });
            */

        });

    }
}
