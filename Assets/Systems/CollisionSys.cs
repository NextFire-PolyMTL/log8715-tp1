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
            
            World.Instance.ForEach<Position>((entity2, position2) =>
            {
                var radius2 = World.Instance.GetComponent<Size>(entity2);
                var velocity2 = World.Instance.GetComponent<Velocity>(entity2);
                if (entity2.Id != entity.Id)
                {
                    if (Mathf.Sqrt(Mathf.Pow((position.Value.X - position2.Value.X),2)
                    + Mathf.Pow((position.Value.Y - position2.Value.Y),2)) <= (radius + radius2.Value.Radius) / 2)
                    {
                        World.Instance.SetComponent<IsColliding>(entity, new IsColliding());
                        var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);
                        if (!collidingWith.HasValue)
                        {
                            World.Instance.SetComponent<CollidingWith>(entity, 
                            new CollidingWith(new int[] {entity2.Id},new Position[] {position2.Value},new Velocity[] {velocity2.Value}, new Size[] {radius2.Value}));
                        }
                        else{
                            //c moche à changer /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
                            int[] collidedShapes = collidingWith.Value.CollidedShapes;
                            int[] newCollidedShapes = new int[collidedShapes.Length+1];
                            Array.Copy(collidedShapes, newCollidedShapes, collidedShapes.Length);
                            newCollidedShapes[collidedShapes.Length]=entity2.Id;

                            Position[] collidedShapesPosition = collidingWith.Value.CollidedShapesPosition;
                            Position[] newCollidedShapesPosition = new Position[collidedShapesPosition.Length+1];
                            Array.Copy(collidedShapesPosition, newCollidedShapesPosition, collidedShapesPosition.Length);
                            newCollidedShapesPosition[collidedShapesPosition.Length]=position2.Value;

                            Velocity[] collidedShapesVelocity = collidingWith.Value.CollidedShapesVelocity;
                            Velocity[] newCollidedShapesVelocity = new Velocity[collidedShapesVelocity.Length+1];
                            Array.Copy(collidedShapesVelocity, newCollidedShapesVelocity, collidedShapesVelocity.Length);
                            newCollidedShapesVelocity[collidedShapesVelocity.Length]=velocity2.Value;
                            
                            Size[] collidedShapesSize = collidingWith.Value.CollidedShapesSize;
                            Size[] newCollidedShapesSize = new Size[collidedShapesSize.Length+1];
                            Array.Copy(collidedShapesSize, newCollidedShapesSize, collidedShapesSize.Length);
                            newCollidedShapesSize[collidedShapesSize.Length]=radius2.Value;

                            World.Instance.SetComponent<CollidingWith>(entity,
                            new CollidingWith(newCollidedShapes,newCollidedShapesPosition,newCollidedShapesVelocity,newCollidedShapesSize));
                        }
                    }
                }
            });
            

        });

    }
}
