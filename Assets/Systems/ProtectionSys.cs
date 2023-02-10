public class ProtectionSys : ISystem
{
    public string Name => nameof(ProtectionSys);

    public void UpdateSystem()
    {
        Config cfg = ECSManager.Instance.Config;
        World world = World.Instance;
        UnityEngine.Random.InitState(cfg.seed);
        world.ForEach<Position>((entity, position) =>
        {
            if (world.GetComponent<IsStatic>(entity).HasValue)
                return;
            if (world.GetComponent<Size>(entity).Value.Scale > cfg.protectionSize)
                return;
            float probaProtection = cfg.protectionProbability;
            if (UnityEngine.Random.value < probaProtection)
                world.SetComponent<IsProtected>(entity, new IsProtected());
        });
    }
}
