using UnityEngine;

public class ExplosionSys : IPhysicSystem
{
    public string Name => nameof(ExplosionSys);

    public void UpdateSystem()
    {
        Utils.PhysicsForEach<Size>((entity, size) =>
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
                World.Instance.SetComponent<Size>(newEntity, new Size(size.Value.Scale >> 1));
                World.Instance.SetComponent<Position>(newEntity, new Position(newPosVit.position1[0], newPosVit.position1[1]));
                World.Instance.SetComponent<Velocity>(newEntity, new Velocity(newPosVit.velocity1[0], newPosVit.velocity1[1]));
                ECSManager.Instance.CreateShape(newEntity.Id, size.Value.Scale >> 1);

                var newEntity2 = World.Instance.CreateEntity();
                World.Instance.SetComponent<Size>(newEntity2, new Size(size.Value.Scale >> 1));
                World.Instance.SetComponent<Position>(newEntity2, new Position(newPosVit.position2[0], newPosVit.position2[1]));
                World.Instance.SetComponent<Velocity>(newEntity2, new Velocity(newPosVit.velocity2[0], newPosVit.velocity2[1]));
                ECSManager.Instance.CreateShape(newEntity2.Id, size.Value.Scale >> 1);


                Utils.AddCommandToBuffer(()=>{
                            World.Instance.DeleteEntity(entity);
                            ECSManager.Instance.DestroyShape(entity.Id);
                });
                
                if (isClicked.HasValue)
                {
                    World.Instance.SetComponent<BornOfClick>(newEntity, new BornOfClick());
                    World.Instance.SetComponent<BornOfClick>(newEntity2, new BornOfClick());
                }
            }
        });
    }
}
