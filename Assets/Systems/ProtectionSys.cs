public class ProtectionSys : IPhysicSystem
{
    public string Name => nameof(ProtectionSys);

    public void UpdateSystem()
    {
        // Config object to lighten the code
        Config cfg = ECSManager.Instance.Config;
        // Loop to setup protection
        Utils.PhysicsForEach<Position>((entity, position) =>
        {
            /* Skips the whole process in three cases
            - The entity is static thus can't be protected
            - The entity is too big
            - The entity is on cooldown (already protected or has been recently)*/
            if (World.Instance.GetComponent<IsStatic>(entity).HasValue ||
                World.Instance.GetComponent<Size>(entity).Value.Scale > cfg.protectionSize ||
                World.Instance.GetComponent<Cooldown>(entity).HasValue)
            {
                return;
            }
            // Gets a random value from the generator initialized in InitSys
            float probaProtection = cfg.protectionProbability;
            if (UnityEngine.Random.value < probaProtection)
            {
                World.Instance.SetComponent<IsProtected>(entity, new IsProtected());
                World.Instance.SetComponent<Cooldown>(entity, new Cooldown(cfg.protectionDuration));
            }
        });

        // Loop to manage Cooldown
        Utils.PhysicsForEach<Cooldown>((entity, cooldown) =>
        {
            // If no cooldown (null pointer) skip execution
            if (!cooldown.HasValue)
            {
                return;
            }
            float newCD = cooldown.Value.Time - UnityEngine.Time.deltaTime;
            bool isProtected = World.Instance.GetComponent<IsProtected>(entity).HasValue;

            // Cooldown ended case
            if (newCD <= 0)
            {
                // If the item is protected we put a new cooldown and remove the protection status
                if (isProtected)
                {
                    World.Instance.RemoveComponent<IsProtected>(entity);
                    World.Instance.SetComponent<Cooldown>(entity, new Cooldown(cfg.protectionCooldown));
                }
                // If it's not protected, means it can be again, so remove the cooldown
                else
                {
                    World.Instance.RemoveComponent<Cooldown>(entity);
                }
            }
            // If the cooldown isn't finished, just decrement it
            else
            {
                World.Instance.SetComponent<Cooldown>(entity, new Cooldown(newCD));
            }
        });
    }
}
