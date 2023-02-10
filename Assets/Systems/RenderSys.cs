using UnityEngine;

public class RenderSys : ISystem
{
    public string Name => "RenderSys";

    public void UpdateSystem()
    {
        World.Instance.ForEach<Position>((entity, position) =>
        {
            ECSManager.Instance.UpdateShapePosition((uint)entity.Id, new Vector2(position.Value.X, position.Value.Y));
        });

        // Est-ce qu'il faudrait mettre l'update du size dans la boucle précédente ? -> pb de maintenabilité ?
        World.Instance.ForEach<Size>((entity, size) =>
        {
            ECSManager.Instance.UpdateShapeSize((uint)entity.Id, size.Value.Scale);
        });

        World.Instance.ForEach<ColorCompo>((entity, colorCompo) =>
        {
            if (colorCompo.HasValue)
            {
                ECSManager.Instance.UpdateShapeColor((uint)entity.Id, colorCompo.Value.ShapeColor);
            }
        });
    }
}
