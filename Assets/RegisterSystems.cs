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
        //SizeSys doit être exécuté avant PositionSys (car sinon CollidingWith des entités seront supprimé)/!\
        toRegister.Add(new SizeSys());
        toRegister.Add(new PositionSys());
        toRegister.Add(new RenderSys());
        toRegister.Add(new ExplosionSys());
        toRegister.Add(new ClickSys());

        return toRegister;
    }
}
