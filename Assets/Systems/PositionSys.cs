using UnityEngine;

public class PositionSys : ISystem
{
    public string Name => "PositionSys";

    public void UpdateSystem()
    {


        World.Instance.ForEach<IsColliding>((entity, isColliding) =>
        {
            /*
            if (!isColliding.HasValue)
            {
                return;
            }
            */
            var position = World.Instance.GetComponent<Position>(entity);
            var velocity = World.Instance.GetComponent<Velocity>(entity);
            var radius = World.Instance.GetComponent<Size>(entity).Value.Radius;
            var collidingWith = World.Instance.GetComponent<CollidingWith>(entity);

            //Trouver mieux /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
            //(et en faire peut-être une cst....)
            var screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

            if (isColliding.HasValue && !collidingWith.HasValue)
            {
                if (Mathf.Abs(position.Value.X) + radius / 2 >= screenBoundary.x)
                {
                    var newPosVit = CollisionUtility.CalculateCollision(
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                        radius,
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(-velocity.Value.Vx, velocity.Value.Vy),
                        radius
                    );
                    World.Instance.SetComponent<Position>(entity, new Position(newPosVit.position1[0], newPosVit.position1[1]));
                    World.Instance.SetComponent<Velocity>(entity, new Velocity(newPosVit.velocity1[0], newPosVit.velocity1[1]));

                }

                if (Mathf.Abs(position.Value.Y) + radius / 2 >= screenBoundary.y)
                {
                    var newPosVit = CollisionUtility.CalculateCollision(
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                        radius,
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(velocity.Value.Vx, -velocity.Value.Vy),
                        radius
                    );
                    World.Instance.SetComponent<Position>(entity, new Position(newPosVit.position2[0], newPosVit.position2[1]));
                    World.Instance.SetComponent<Velocity>(entity, new Velocity(newPosVit.velocity2[0], newPosVit.velocity2[1]));
                }

                World.Instance.RemoveComponent<IsColliding>(entity);
            }
            //code ci-dessous à discuter
            /*
            if(isColliding.HasValue && collidingWith.HasValue){
                Debug.Log("ça marche");
                var collidedShapes=collidingWith.Value.CollidedShapes;
                foreach (var collidedShape in collidedShapes){

                    var position2 = World.Instance.GetComponent<Position>(collidedShape);
                    var velocity2 = World.Instance.GetComponent<Velocity>(collidedShape);
                    var radius2 = World.Instance.GetComponent<Size>(collidedShape).Value.Radius;
                    var newPosVit = CollisionUtility.CalculateCollision(
                        new Vector2(position.Value.X, position.Value.Y),
                        new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                        radius,
                        new Vector2(position2.Value.X, position2.Value.Y),
                        new Vector2(velocity2.Value.Vx, velocity2.Value.Vy),
                        radius2
                    );
                    World.Instance.SetComponent<Position>(entity, new Position(newPosVit.position1[0], newPosVit.position1[1]));
                    World.Instance.SetComponent<Velocity>(entity, new Velocity(newPosVit.velocity1[0], newPosVit.velocity1[1]));
                
                }
                World.Instance.RemoveComponent<IsColliding>(entity);
                World.Instance.RemoveComponent<CollidingWith>(entity);
                
            }
            */

        });

        World.Instance.ForEach<Velocity>((entity, velocity) =>
        {
            var position = World.Instance.GetComponent<Position>(entity);
            World.Instance.SetComponent<Position>(entity, new Position(position.Value.X + velocity.Value.Vx, position.Value.Y + velocity.Value.Vy));
        });
    }
}
