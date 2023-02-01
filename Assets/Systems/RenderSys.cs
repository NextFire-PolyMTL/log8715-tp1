public class RenderSys : ISystem
{
    public string Name => "RenderSys";

    public void UpdateSystem()
    {
        World.Instance.ForEach<Position>((entity, position) =>
        {
            ECSManager.Instance.UpdateShapePosition((uint)entity.Id, new UnityEngine.Vector2(position.X, position.Y));
        });
    }
}
