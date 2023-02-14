using UnityEngine;

public class ExplosionSys : IPhysicSystem
{
    public string Name => nameof(ExplosionSys);

    public void UpdateSystem()
    {
        Utils.PhysicsForEach<Size>((entity, size) =>
        {
            var isClicked = World.Instance.GetComponent<IsClicked>(entity);

            /*If the user has clicked on the object and its size is bigger than 2
            or if the object has reached the explosion size
            */
            if (isClicked.HasValue && (size.Value.Scale == 1))
            {
                //Deletes the entity if it's clicked and its size is 1
                Utils.AddCommandToBuffer(() =>
                {
                    World.Instance.DeleteEntity(entity);
                    ECSManager.Instance.DestroyShape(entity.Id);
                });
            }
            else if ((isClicked.HasValue && size.Value.Scale >= 2) || size.Value.Scale >= ECSManager.Instance.Config.explosionSize)
            {

                var position = World.Instance.GetComponent<Position>(entity);
                var velocity = World.Instance.GetComponent<Velocity>(entity);

                /*To simulate the explosion, we consider the result of a collision bewteen the object
                and another shadow object with the same configuration but having an opposite speed*/
                var newPosVit = CollisionUtility.CalculateCollision(
                    new Vector2(position.Value.X, position.Value.Y),
                    new Vector2(velocity.Value.Vx, velocity.Value.Vy),
                    size.Value.Scale,
                    new Vector2(position.Value.X, position.Value.Y),
                    new Vector2(-velocity.Value.Vx, -velocity.Value.Vy),
                    size.Value.Scale
                );

                //We instanciate the 2 resulting enities of the explosion by using the result of the false collision simulated before

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


                /*If the explosion is due to a click, we set BornOfClick tag for the two new entity
                It will be useful to set their color as pink in the ColorSys*/
                if (isClicked.HasValue)
                {
                    World.Instance.SetComponent<BornOfClick>(newEntity, new BornOfClick());
                    World.Instance.SetComponent<BornOfClick>(newEntity2, new BornOfClick());
                }

                //We delete the entity after the execution of other systems in another system which execute the singleton CommandBuffer
                Utils.AddCommandToBuffer(() =>
                {
                    World.Instance.DeleteEntity(entity);
                    ECSManager.Instance.DestroyShape(entity.Id);
                });
            }
        });
    }
}
