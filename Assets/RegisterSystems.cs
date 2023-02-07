using System.Collections.Generic;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        // determine order of systems to add
        var toRegister = new List<ISystem>();

        // Add your systems here
        toRegister.Add(new InitSys());
        
        toRegister.Add(new CollisionSys());
        toRegister.Add(new PositionSys());
        toRegister.Add(new RenderSys());
        toRegister.Add(new ExplosionSys());
        toRegister.Add(new ClickSys());

        return toRegister;
    }
}
