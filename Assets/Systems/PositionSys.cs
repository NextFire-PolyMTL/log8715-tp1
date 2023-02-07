using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PositionSys : ISystem
{
    public string Name => "PositionSys";

    public void UpdateSystem()
    {
        World.Instance.ForEach<Velocity>((entity, velocity) =>
        {
            var position = World.Instance.GetComponent<Position>(entity);
            World.Instance.SetComponent<Position>(entity, new Position(position.X + velocity.Vx, position.Y + velocity.Vy));
        });

        World.Instance.ForEach<IsColliding>((entity,IsColliding) =>
        {

            var position = World.Instance.GetComponent<Position>(entity);
            var velocity = World.Instance.GetComponent<Velocity>(entity);
            var radius = World.Instance.GetComponent<Size>(entity).Radius;
            var collidedShapes = World.Instance.GetComponent<CollidingWith>(entity).CollidedShapes;
            //Trouver mieux /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
            //(et en faire peut-être une cst....)
            var screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            if (!collidedShapes.Equals(default(CollidingWith)))
            {
                if (Mathf.Abs(position.X) + radius >= screenBoundary.x)
                {
                    var newPosVit = CollisionUtility.CalculateCollision(
                        new Vector2(position.X, position.Y), new Vector2(velocity.Vx, velocity.Vy), radius,
                    new Vector2(position.X, position.Y), new Vector2(-velocity.Vx, velocity.Vy), radius);
                    World.Instance.SetComponent<Position>(entity, new Position(newPosVit.position1[0], newPosVit.position1[1]));
                    World.Instance.SetComponent<Velocity>(entity, new Velocity(newPosVit.velocity1[0], newPosVit.velocity1[1]));
                    
                }
                if (Mathf.Abs(position.Y) + radius >= screenBoundary.y)
                {
                    var newPosVit = CollisionUtility.CalculateCollision(
                        new Vector2(position.X, position.Y), new Vector2(velocity.Vx, velocity.Vy), radius,
                    new Vector2(position.X, position.Y), new Vector2(velocity.Vx, -velocity.Vy), radius);
                    World.Instance.SetComponent<Position>(entity, new Position(newPosVit.position2[0], newPosVit.position2[1]));
                    World.Instance.SetComponent<Velocity>(entity, new Velocity(newPosVit.velocity2[0], newPosVit.velocity2[1]));

                }
                World.Instance.RemoveComponent<IsColliding>(entity);
            }
        });
    }
}
