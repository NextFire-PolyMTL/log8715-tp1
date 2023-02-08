using UnityEngine;

public class ExplosionSys : ISystem
{
    public string Name => "ExplosionSys";

    public void UpdateSystem()
    {
        World.Instance.ForEach<Size>((entity, size) =>
        {
            var isClicked = World.Instance.GetComponent<IsClicked>(entity);
            if ((isClicked.HasValue && size.Value.Scale >= 2) || size.Value.Scale >= ECSManager.Instance.Config.explosionSize)
            {
                //var size = World.Instance.GetComponent<Size>(entity);
                var position = World.Instance.GetComponent<Position>(entity);
                var velocity = World.Instance.GetComponent<Velocity>(entity);

                //var newVelocity=new Velocity(velocity.Vx, velocity.Vy);
                var newPosVit = CollisionUtility.CalculateCollision(
                    new Vector2(position.Value.X, position.Value.Y),
                    new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                    size.Value.Scale,
                    new Vector2(position.Value.X, position.Value.Y),
                    new Vector2(-velocity.Value.Vx, -velocity.Value.Vy),
                    size.Value.Scale
                );
                var newEntity = World.Instance.CreateEntity();
                World.Instance.SetComponent<Size>(newEntity, new Size(size.Value.Scale>>1));
                World.Instance.SetComponent<Position>(newEntity, new Position(newPosVit.position1[0], newPosVit.position1[1]));
                World.Instance.SetComponent<Velocity>(newEntity, new Velocity(newPosVit.velocity1[0], newPosVit.velocity1[1]));
                ECSManager.Instance.CreateShape((uint)newEntity.Id, (int)size.Value.Scale / 2);

                newEntity = World.Instance.CreateEntity();
                World.Instance.SetComponent<Size>(newEntity, new Size(size.Value.Scale>>1));
                World.Instance.SetComponent<Position>(newEntity, new Position(newPosVit.position2[0], newPosVit.position2[1]));
                World.Instance.SetComponent<Velocity>(newEntity, new Velocity(newPosVit.velocity2[0], newPosVit.velocity2[1]));
                ECSManager.Instance.CreateShape((uint)newEntity.Id, (int)size.Value.Scale / 2);

                World.Instance.DeleteEntity(entity);
                ECSManager.Instance.DestroyShape((uint)entity.Id);
            }
        });
    }
}
