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
            // If the entity is already protected
            if (world.GetComponent<IsStatic>(entity).HasValue ||
                world.GetComponent<Size>(entity).Value.Scale > cfg.protectionSize ||
                world.GetComponent<IsProtected>(entity).HasValue)
                return;
            float probaProtection = cfg.protectionProbability;
            if (UnityEngine.Random.value < probaProtection)
            {
                world.SetComponent<IsProtected>(entity, new IsProtected());
                world.SetComponent<Cooldown>(entity, new Cooldown(cfg.protectionDuration));
            }
        });
        // Ugh, doesn't work with a loop over each Component for whatever reason.
        // TODO fix
        Utils.PhysicsForEach<Position>((entity, Position) =>
        {
            // Then I test if it has a CD compo here, very inefficient
            if (!world.GetComponent<Cooldown>(entity).HasValue)
                return;
            var cooldown = world.GetComponent<Cooldown>(entity);
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
                world.SetComponent<Cooldown>(entity, new Cooldown(newCD));
        });
    }
}
