using System.Collections.Generic;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        // determine order of systems to add
        var toRegister = new List<ISystem>();

        // Add your systems here

        /* Initialization */
        toRegister.Add(new InitSys());

        /* Before physics */
        toRegister.Add(new BacktrackSys());
        toRegister.Add(new ClickSys());

        /* Physics */
        var physicSystems = new IPhysicSystem[] {
            new CollisionSys(),
            // SizeSys doit être exécuté avant PositionSys
            // (car sinon CollidingWith des entités seront supprimé)
            new SizeSys(),
            new ColorSys(),
            new PositionSys(),
            new ExplosionSys(),
            new ProtectionSys()
        };
        // First pass on everything
        toRegister.Add(new PhysicsIgnoreResetSys());
        toRegister.AddRange(physicSystems);
        // Three more passes ignoring the right side
        toRegister.Add(new PhysicsIgnoreSys());
        toRegister.AddRange(physicSystems);
        toRegister.AddRange(physicSystems);
        toRegister.AddRange(physicSystems);

        /* Rendering */
        toRegister.Add(new RenderSys());
        toRegister.Add(new CommandBufferSys());

        return toRegister;
    }
}
