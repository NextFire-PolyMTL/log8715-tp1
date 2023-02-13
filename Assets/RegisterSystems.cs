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
            new ColorSys(),
            new PositionSys(),
            new SizeSys(),
            new ExplosionSys(),
            new ProtectionSys(),
        };
        var cmdBufferSys = new CommandBufferSys();
        // 1st pass on everything
        toRegister.Add(new PhysicsIgnoreResetSys());
        toRegister.AddRange(physicSystems);
        toRegister.Add(cmdBufferSys);
        // 3 more passes ignoring the right side
        toRegister.Add(new PhysicsIgnoreSys());
        // 2nd pass
        toRegister.AddRange(physicSystems);
        toRegister.Add(cmdBufferSys);
        // 3rd pass
        toRegister.AddRange(physicSystems);
        toRegister.Add(cmdBufferSys);
        // 4th pass
        toRegister.AddRange(physicSystems);
        toRegister.Add(cmdBufferSys);

        /* Rendering */
        toRegister.Add(new RenderSys());

        return toRegister;
    }
}
