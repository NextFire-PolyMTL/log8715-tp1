using System.Linq;

public class LeftSideSys : ISystem
{
    public string Name => nameof(LeftSideSys);

    const uint NB_ADDED_SIMULATIONS = 3;

    public void UpdateSystem()
    {
        World.Instance.ForEach<Position>((entity, position) =>
        {
            if (position.Value.X > 0)
            {
                World.Instance.SetComponent<PhysicsIgnore>(entity, new PhysicsIgnore());
            }
        });

        var allSystems = RegisterSystems.GetListOfSystems();
        var enabledPhysicSystems = allSystems.Where(system =>
            ECSManager.Instance.Config.SystemsEnabled[system.Name]
            && system is IPhysicSystem
        );
        for (int i = 0; i < NB_ADDED_SIMULATIONS; i++)
        {
            foreach (var system in enabledPhysicSystems)
            {
                system.UpdateSystem();
            }
        }

        World.Instance.ForEach<PhysicsIgnore>((entity, val) =>
        {
            if (val.HasValue)
            {
                World.Instance.RemoveComponent<PhysicsIgnore>(entity);
            }
        });
    }
}
