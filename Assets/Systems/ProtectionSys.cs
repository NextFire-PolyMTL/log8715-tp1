public class ProtectionSys : IPhysicSystem
{
    public string Name => nameof(ProtectionSys);

    public void UpdateSystem()
    {
        Config cfg = ECSManager.Instance.Config;
        World world = World.Instance;
        Utils.PhysicsForEach<Position>((entity, position) =>
        {
            // If the entity is static it can't be protected
            // If the entity is too big
            // If the entity has a CD (by being protected or waiting)
            if (world.GetComponent<IsStatic>(entity).HasValue ||
                world.GetComponent<Size>(entity).Value.Scale > cfg.protectionSize ||
                world.GetComponent<Cooldown>(entity).HasValue)
            {
                return;
            }
            float probaProtection = cfg.protectionProbability;
            if (UnityEngine.Random.value < probaProtection)
            {
                world.SetComponent<IsProtected>(entity, new IsProtected());
                world.SetComponent<Cooldown>(entity, new Cooldown(cfg.protectionDuration));
            }
        });

        Utils.PhysicsForEach<Cooldown>((entity, cooldown) =>
        {
            if (!cooldown.HasValue)
            {
                return;
            }
            float newCD = cooldown.Value.Time - UnityEngine.Time.deltaTime;
            bool isProtected = world.GetComponent<IsProtected>(entity).HasValue;

            if (newCD <= 0)
            {
                if (isProtected)
                {
                    world.RemoveComponent<IsProtected>(entity);
                    world.SetComponent<Cooldown>(entity, new Cooldown(cfg.protectionCooldown));
                }
                else
                {
                    world.RemoveComponent<Cooldown>(entity);
                }
            }
            else
            {
                world.SetComponent<Cooldown>(entity, new Cooldown(newCD));
            }
        });
    }
}
